using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Tree;

namespace StoryTree.Gui.ViewModels
{
    public class ClassesProbabilitySpecificationViewModel : ProbabilitySpecificationViewModelBase
    {
        public ClassesProbabilitySpecificationViewModel(TreeEvent treeEvent, Project project) : base(treeEvent)
        {
            Project = project;
        }

        public Project Project { get; }

        public ObservableCollection<ExpertClassEstimation> ClassesProbabilitySpecification => TreeEvent?.ClassesProbabilitySpecification;

        public DataTable MeanEstimationsList => CreateEstimationDataTable(
            estimation => (int) estimation.AverageEstimation,
            (o, e) => { DataTableRowChanged(e, SetValueActionAverage); });

        public DataTable MaxEstimationsList => CreateEstimationDataTable(
            estimation => (int)estimation.MaxEstimation,
            (o, e) => { DataTableRowChanged(e, SetValueActionMaximum); });

        public DataTable MinEstimationsList => CreateEstimationDataTable(
            estimation => (int)estimation.MinEstimation,
            (o, e) => { DataTableRowChanged(e, SetValueActionMinimum); });

        protected override void OnTreeEventPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(TreeEvent.ClassesProbabilitySpecification):
                    OnPropertyChanged(nameof(MeanEstimationsList));
                    OnPropertyChanged(nameof(MaxEstimationsList));
                    OnPropertyChanged(nameof(MinEstimationsList));
                    break;
            }
        }

        private DataTable CreateEstimationDataTable(Func<ExpertClassEstimation, int> getValueFunc, DataRowChangeEventHandler rowChangeEventHandler)
        {
            var dataTable = new DataTable();
            var waterLevelColumn = new DataColumn("Waterstand")
            {
                DataType = typeof(double),
            };
            dataTable.Columns.Add(waterLevelColumn);
            dataTable.Columns.AddRange(Project.Experts.Select(e => new DataColumn(e.Name) {DataType = typeof(int)}).ToArray());

            var hydraulicConditions = Project.HydraulicConditions.Distinct().OrderBy(hc => hc.WaterLevel).ToArray();
            for (int iRow = 0; iRow < hydraulicConditions.Length; iRow++)
            {
                var hydraulicCondition = hydraulicConditions[iRow];
                dataTable.Rows.Add(hydraulicCondition.WaterLevel);
                for (var i = 0; i < Project.Experts.Count; i++)
                {
                    var expert = Project.Experts[i];
                    var specification = ClassesProbabilitySpecification
                        .FirstOrDefault(e => e.HydraulicCondition == hydraulicCondition && e.Expert == expert);
                    if (specification == null)
                    {
                        specification = new ExpertClassEstimation
                        {
                            AverageEstimation = ProbabilityClass.None,
                            MinEstimation = ProbabilityClass.None,
                            MaxEstimation = ProbabilityClass.None,
                            HydraulicCondition = hydraulicCondition,
                            Expert = expert
                        };
                        ClassesProbabilitySpecification.Add(specification);
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
                var specification = ClassesProbabilitySpecification
                    .FirstOrDefault(sp =>
                        sp.Expert == Project.Experts.ElementAt(i) && Math.Abs(sp.HydraulicCondition.WaterLevel - waterLevel) < 1e-8);
                setValueAction(specification, probabilityClass);
            }
        }
    }
}