using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    public class PersistenceRegistry
    {
        private readonly Dictionary<Project, ProjectEntity> projects = CreateDictionary<Project, ProjectEntity>();
        private readonly Dictionary<Expert, ExpertEntity> experts = CreateDictionary<Expert, ExpertEntity>();
        private readonly Dictionary<Person, PersonEntity> persons = CreateDictionary<Person, PersonEntity>();
        private readonly Dictionary<HydraulicCondition, HydraulicConditionElementEntity> hydraulicConditions = CreateDictionary<HydraulicCondition, HydraulicConditionElementEntity>();
        private readonly Dictionary<FragilityCurveElement, FragilityCurveElementEntity> fragilityCurveElements = CreateDictionary<FragilityCurveElement, FragilityCurveElementEntity>();
        private readonly Dictionary<EventTree, EventTreeEntity> eventTrees = CreateDictionary<EventTree, EventTreeEntity>();
        private readonly Dictionary<TreeEvent, TreeEventEntity> treeEvents = CreateDictionary<TreeEvent, TreeEventEntity>();

        #region Register Methods

        internal void Register(Project model, ProjectEntity entity)
        {
            Register(projects, model, entity);
        }

        internal void Register(Expert model, ExpertEntity entity)
        {
            Register(experts, model, entity);
        }
        internal void Register(Person model, PersonEntity entity)
        {
            Register(persons, model, entity);
        }
        internal void Register(HydraulicCondition model, HydraulicConditionElementEntity entity)
        {
            Register(hydraulicConditions, model, entity);
        }
        internal void Register(FragilityCurveElement model, FragilityCurveElementEntity entity)
        {
            Register(fragilityCurveElements, model, entity);
        }
        internal void Register(EventTree model, EventTreeEntity entity)
        {
            Register(eventTrees, model, entity);
        }
        internal void Register(TreeEvent model, TreeEventEntity entity)
        {
            Register(treeEvents, model, entity);
        }


        #endregion

        #region Contains Methods

        internal bool Contains(Project model)
        {
            return ContainsValue(projects, model);
        }

        internal bool Contains(Expert model)
        {
            return ContainsValue(experts, model);
        }
        internal bool Contains(Person model)
        {
            return ContainsValue(persons, model);
        }
        internal bool Contains(HydraulicCondition model)
        {
            return ContainsValue(hydraulicConditions, model);
        }
        internal bool Contains(FragilityCurveElement model)
        {
            return ContainsValue(fragilityCurveElements, model);
        }
        internal bool Contains(EventTree model)
        {
            return ContainsValue(eventTrees, model);
        }
        internal bool Contains(TreeEvent model)
        {
            return ContainsValue(treeEvents, model);
        }
        #endregion

        #region Get Methods

        public ProjectEntity Get(Project model)
        {
            return Get(projects, model);
        }

        public ExpertEntity Get(Expert model)
        {
            return Get(experts, model);
        }
        public PersonEntity Get(Person model)
        {
            return Get(persons, model);
        }
        public HydraulicConditionElementEntity Get(HydraulicCondition model)
        {
            return Get(hydraulicConditions, model);
        }
        public FragilityCurveElementEntity Get(FragilityCurveElement model)
        {
            return Get(fragilityCurveElements, model);
        }
        public EventTreeEntity Get(EventTree model)
        {
            return Get(eventTrees, model);
        }
        public TreeEventEntity Get(TreeEvent model)
        {
            return Get(treeEvents, model);
        }
        #endregion

        #region helpers
        private static Dictionary<TEntity, TModel> CreateDictionary<TEntity, TModel>()
        {
            return new Dictionary<TEntity, TModel>(new ReferenceEqualityComparer<TEntity>());
        }

        private bool ContainsValue<TModel, TEntity>(Dictionary<TModel, TEntity> collection, TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return collection.Keys.Contains(model, new ReferenceEqualityComparer<TModel>());
        }

        private void Register<TModel, TEntity>(Dictionary<TModel, TEntity> collection, TModel model, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            collection[model] = entity;
        }

        private TEntity Get<TModel, TEntity>(Dictionary<TModel, TEntity> collection, TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return collection[model];
        }
        #endregion
    }
}