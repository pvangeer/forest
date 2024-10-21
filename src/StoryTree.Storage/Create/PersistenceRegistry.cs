using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage.Create
{
    public class PersistenceRegistry
    {
        private readonly Dictionary<Data.EventTreeProject, ProjectXmlEntity> projects = CreateDictionary<Data.EventTreeProject, ProjectXmlEntity>();
        private readonly Dictionary<Expert, ExpertXmlEntity> experts = CreateDictionary<Expert, ExpertXmlEntity>();
        private readonly Dictionary<Person, PersonXmlEntity> persons = CreateDictionary<Person, PersonXmlEntity>();
        private readonly Dictionary<HydraulicCondition, HydraulicConditionXmlEntity> hydraulicConditions = CreateDictionary<HydraulicCondition, HydraulicConditionXmlEntity>();
        private readonly Dictionary<FragilityCurveElement, FragilityCurveElementXmlEntity> fragilityCurveElements = CreateDictionary<FragilityCurveElement, FragilityCurveElementXmlEntity>();
        private readonly Dictionary<EventTree, EventTreeXmlEntity> eventTrees = CreateDictionary<EventTree, EventTreeXmlEntity>();
        private readonly Dictionary<TreeEvent, TreeEventXmlEntity> treeEvents = CreateDictionary<TreeEvent, TreeEventXmlEntity>();

        #region Register Methods

        internal void Register(Data.EventTreeProject model, ProjectXmlEntity entity)
        {
            Register(projects, model, entity);
        }

        internal void Register(Expert model, ExpertXmlEntity entity)
        {
            Register(experts, model, entity);
        }
        internal void Register(Person model, PersonXmlEntity entity)
        {
            Register(persons, model, entity);
        }
        internal void Register(HydraulicCondition model, HydraulicConditionXmlEntity entity)
        {
            Register(hydraulicConditions, model, entity);
        }
        internal void Register(FragilityCurveElement model, FragilityCurveElementXmlEntity entity)
        {
            Register(fragilityCurveElements, model, entity);
        }
        internal void Register(EventTree model, EventTreeXmlEntity entity)
        {
            Register(eventTrees, model, entity);
        }
        internal void Register(TreeEvent model, TreeEventXmlEntity entity)
        {
            Register(treeEvents, model, entity);
        }


        #endregion

        #region Contains Methods

        internal bool Contains(EventTreeProject model)
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

        public ProjectXmlEntity Get(Data.EventTreeProject model)
        {
            return Get(projects, model);
        }

        public ExpertXmlEntity Get(Expert model)
        {
            return Get(experts, model);
        }
        public PersonXmlEntity Get(Person model)
        {
            return Get(persons, model);
        }
        public HydraulicConditionXmlEntity Get(HydraulicCondition model)
        {
            return Get(hydraulicConditions, model);
        }
        public FragilityCurveElementXmlEntity Get(FragilityCurveElement model)
        {
            return Get(fragilityCurveElements, model);
        }
        public EventTreeXmlEntity Get(EventTree model)
        {
            return Get(eventTrees, model);
        }
        public TreeEventXmlEntity Get(TreeEvent model)
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