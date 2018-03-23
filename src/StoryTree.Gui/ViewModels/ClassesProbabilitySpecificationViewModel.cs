using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Estimations.Classes;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ClassesProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public ClassesProbabilitySpecificationViewModel(ClassesProbabilitySpecification probabilitySpecification, Project project) :
            base(probabilitySpecification)
        {
            ClassesProbabilitySpecification = probabilitySpecification;
            Project = project;
            project.Experts.CollectionChanged += ExpertsCollectionChanged;
        }

        private void WaterLevelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: Is this necessary? => Manipulate estimation lists
        }

        private void ExpertsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: Is this necessary? => Manipulate estimation lists
        }

        public Project Project { get; }

        public ClassesProbabilitySpecification ClassesProbabilitySpecification { get; }

        public DataTable MeanEstimationsList => CreateEstimationDataTable(
            estimation => (int) estimation.AverageEstimation,
            (o, e) => { DataTableRowChanged(e, SetValueActionAverage); });

        public DataTable MaxEstimationsList => CreateEstimationDataTable(
            estimation => (int)estimation.MaxEstimation,
            (o, e) => { DataTableRowChanged(e, SetValueActionMaximum); });

        public DataTable MinEstimationsList => CreateEstimationDataTable(
            estimation => (int)estimation.MinEstimation,
            (o, e) => { DataTableRowChanged(e, SetValueActionMinimum); });

        private DataTable CreateEstimationDataTable(Func<ExpertClassEstimation, int> getValueFunc, DataRowChangeEventHandler rowChangeEventHandler)
        {
            var dataTable = new DataTable();
            var waterLevelColumn = new DataColumn("Waterstand")
            {
                DataType = typeof(double),
            };
            dataTable.Columns.Add(waterLevelColumn);
            dataTable.Columns.AddRange(Project.Experts.Select(e => new DataColumn(e.Name) {DataType = typeof(int)}).ToArray());

            for (int iRow = 0; iRow < Project.WaterLevels.Count(); iRow++)
            {
                var waterLevel = Project.WaterLevels.ElementAt(iRow);
                dataTable.Rows.Add(waterLevel);
                for (var i = 0; i < Project.Experts.Count; i++)
                {
                    var expert = Project.Experts[i];
                    var specification = ClassesProbabilitySpecification.Estimations
                        .FirstOrDefault(e => Math.Abs(e.WaterLevel - waterLevel) < 1e-8 && e.Expert == expert);
                    if (specification == null)
                    {
                        specification = new ExpertClassEstimation
                        {
                            AverageEstimation = ProbabilityClass.None,
                            MinEstimation = ProbabilityClass.None,
                            MaxEstimation = ProbabilityClass.None,
                            WaterLevel = waterLevel,
                            Expert = expert
                        };
                        ClassesProbabilitySpecification.Estimations.Add(specification);
                    }

                    dataTable.Rows[iRow][i + 1] = getValueFunc(specification);
                }
            }

            waterLevelColumn.ReadOnly = true;
            dataTable.RowChanged += rowChangeEventHandler;
            return dataTable;
        }

        private void SetValueActionAverage(ExpertClassEstimation specification, ProbabilityClass probabilityClass)
        {
            if (specification != null && specification.AverageEstimation != probabilityClass)
            {
                specification.AverageEstimation = probabilityClass;
            }
        }

        private void SetValueActionMaximum(ExpertClassEstimation specification, ProbabilityClass probabilityClass)
        {
            if (specification != null && specification.MaxEstimation != probabilityClass)
            {
                specification.MaxEstimation = probabilityClass;
            }
        }

        private void SetValueActionMinimum(ExpertClassEstimation specification, ProbabilityClass probabilityClass)
        {
            if (specification != null && specification.MinEstimation != probabilityClass)
            {
                specification.MinEstimation = probabilityClass;
            }
        }

        private void DataTableRowChanged(DataRowChangeEventArgs e, Action<ExpertClassEstimation,ProbabilityClass> setValueAction)
        {
            var waterLevel = (double)e.Row.ItemArray[0];
            for (int i = 0; i < Project.Experts.Count; i++)
            {
                var specifiedIntegerValue = (int)e.Row.ItemArray[i + 1];
                if (!Enum.IsDefined(typeof(ProbabilityClass), specifiedIntegerValue))
                {
                    throw new ArgumentOutOfRangeException("",specifiedIntegerValue,"Het is alleen toegestaan om classen tussen 1 en 7 te specificeren.");
                }
                
                var probabilityClass = (ProbabilityClass)specifiedIntegerValue;
                var specification = ClassesProbabilitySpecification.Estimations
                    .FirstOrDefault(sp =>
                        sp.Expert == Project.Experts.ElementAt(i) && Math.Abs(sp.WaterLevel - waterLevel) < 1e-8);
                setValueAction(specification, probabilityClass);
            }
        }
    }
}