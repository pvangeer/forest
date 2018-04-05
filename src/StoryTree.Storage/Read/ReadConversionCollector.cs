using System;
using System.Collections.Generic;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Read
{
    internal class ReadConversionCollector
    {
        private readonly Dictionary<PersonEntity, Person> persons = CreateDictionary<PersonEntity, Person>();
        private readonly Dictionary<ExpertEntity, Expert> experts = CreateDictionary<ExpertEntity, Expert>();
        private readonly Dictionary<HydraulicConditionElementEntity, HydraulicCondition> hydraulicConditions = 
            CreateDictionary<HydraulicConditionElementEntity, HydraulicCondition>();
        private readonly Dictionary<FragilityCurveElementEntity, FragilityCurveElement> fragilityCurveElements =
            CreateDictionary<FragilityCurveElementEntity, FragilityCurveElement>();
        private readonly Dictionary<EventTreeEntity, EventTree> eventTrees = CreateDictionary<EventTreeEntity, EventTree>();
        private readonly Dictionary<TreeEventEntity, TreeEvent> treeEvents = CreateDictionary<TreeEventEntity, TreeEvent>();
        private readonly Dictionary<ExpertClassEstimationEntity, ExpertClassEstimation> expertClassEstimations =
            CreateDictionary<ExpertClassEstimationEntity, ExpertClassEstimation>();

        
        internal void Collect(PersonEntity entity, Person model)
        {
            Collect(persons,entity,model);
        }
        internal void Collect(ExpertEntity entity, Expert model)
        {
            Collect(experts, entity, model);
        }
        internal void Collect(HydraulicConditionElementEntity entity, HydraulicCondition model)
        {
            Collect(hydraulicConditions, entity, model);
        }
        internal void Collect(FragilityCurveElementEntity entity, FragilityCurveElement model)
        {
            Collect(fragilityCurveElements, entity, model);
        }
        internal void Collect(EventTreeEntity entity, EventTree model)
        {
            Collect(eventTrees, entity, model);
        }
        internal void Collect(TreeEventEntity entity, TreeEvent model)
        {
            Collect(treeEvents, entity, model);
        }
        internal void Collect(ExpertClassEstimationEntity entity, ExpertClassEstimation model)
        {
            Collect(expertClassEstimations, entity, model);
        }

        internal bool Contains(PersonEntity entity)
        {
            return Contains(persons, entity);
        }
        internal bool Contains(ExpertEntity entity)
        {
            return Contains(experts, entity);
        }
        internal bool Contains(HydraulicConditionElementEntity entity)
        {
            return Contains(hydraulicConditions, entity);
        }
        internal bool Contains(FragilityCurveElementEntity entity)
        {
            return Contains(fragilityCurveElements, entity);
        }
        internal bool Contains(EventTreeEntity entity)
        {
            return Contains(eventTrees, entity);
        }
        internal bool Contains(TreeEventEntity entity)
        {
            return Contains(treeEvents, entity);
        }
        internal bool Contains(ExpertClassEstimationEntity entity)
        {
            return Contains(expertClassEstimations, entity);
        }

        internal Person Get(PersonEntity entity)
        {
            return Get(persons, entity);
        }
        internal Expert Get(ExpertEntity entity)
        {
            return Get(experts, entity);
        }
        internal HydraulicCondition Get(HydraulicConditionElementEntity entity)
        {
            return Get(hydraulicConditions, entity);
        }
        internal FragilityCurveElement Get(FragilityCurveElementEntity entity)
        {
            return Get(fragilityCurveElements, entity);
        }
        internal EventTree Get(EventTreeEntity entity)
        {
            return Get(eventTrees, entity);
        }
        internal TreeEvent Get(TreeEventEntity entity)
        {
            return Get(treeEvents, entity);
        }
        internal ExpertClassEstimation Get(ExpertClassEstimationEntity entity)
        {
            return Get(expertClassEstimations, entity);
        }
        #region helpers

        private TModel Get<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                return collection[entity];
            }
            catch (KeyNotFoundException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        private bool Contains<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return collection.ContainsKey(entity);
        }

        private void Collect<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TEntity entity, TModel model)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            collection[entity] = model;
        }

        private static Dictionary<TEntity, TModel> CreateDictionary<TEntity, TModel>()
        {
            return new Dictionary<TEntity, TModel>(new ReferenceEqualityComparer<TEntity>());
        }
        #endregion
    }
}