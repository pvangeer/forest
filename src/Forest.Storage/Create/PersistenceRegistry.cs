﻿using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Create
{
    public class PersistenceRegistry
    {
        private readonly Dictionary<EventTree, EventTreeXmlEntity> eventTrees = CreateDictionary<EventTree, EventTreeXmlEntity>();

        private readonly Dictionary<FragilityCurveElement, FragilityCurveElementXmlEntity> fragilityCurveElements =
            CreateDictionary<FragilityCurveElement, FragilityCurveElementXmlEntity>();

        private readonly Dictionary<Person, PersonXmlEntity> persons = CreateDictionary<Person, PersonXmlEntity>();
        private readonly Dictionary<TreeEvent, TreeEventXmlEntity> treeEvents = CreateDictionary<TreeEvent, TreeEventXmlEntity>();

        private int idCount;

        #region Register Methods

        public PersistenceRegistry()
        {
            Reset();
        }

        public void Reset()
        {
            idCount = 1;
        }

        internal void Register(Person model, PersonXmlEntity entity)
        {
            Register(persons, model, entity);
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

        internal void Register<TEntity>(TEntity entity) where TEntity : XmlEntityBase
        {
            entity.Id = idCount;
            idCount += 1;
        }
        #endregion

        #region Contains Methods

        internal bool Contains(Person model)
        {
            return ContainsValue(persons, model);
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

        public PersonXmlEntity Get(Person model)
        {
            return Get(persons, model);
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
                throw new ArgumentNullException(nameof(model));

            return collection.Keys.Contains(model, new ReferenceEqualityComparer<TModel>());
        }

        private void Register<TModel, TEntity>(Dictionary<TModel, TEntity> collection, TModel model, TEntity entity) where TEntity : XmlEntityBase
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            collection[model] = entity;
            entity.Id = idCount;
            idCount += 1;
        }

        private TEntity Get<TModel, TEntity>(Dictionary<TModel, TEntity> collection, TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return collection[model];
        }

        #endregion
    }
}