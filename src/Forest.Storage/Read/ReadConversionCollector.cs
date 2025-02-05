﻿using System;
using System.Collections.Generic;
using System.Linq;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Probabilities;
using Forest.Data.Tree;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Read
{
    internal class ReadConversionCollector
    {
        private readonly Dictionary<EventTreeXmlEntity, EventTree> eventTrees = CreateDictionary<EventTreeXmlEntity, EventTree>();

        private readonly Dictionary<FragilityCurveElementXmlEntity, FragilityCurveElement> fragilityCurveElements =
            CreateDictionary<FragilityCurveElementXmlEntity, FragilityCurveElement>();

        private readonly Dictionary<PersonXmlEntity, Person> persons = CreateDictionary<PersonXmlEntity, Person>();
        private readonly Dictionary<TreeEventXmlEntity, TreeEvent> treeEvents = CreateDictionary<TreeEventXmlEntity, TreeEvent>();


        internal void Collect(PersonXmlEntity entity, Person model)
        {
            Collect(persons, entity, model);
        }

        internal void Collect(FragilityCurveElementXmlEntity entity, FragilityCurveElement model)
        {
            Collect(fragilityCurveElements, entity, model);
        }

        internal void Collect(EventTreeXmlEntity entity, EventTree model)
        {
            Collect(eventTrees, entity, model);
        }

        internal void Collect(TreeEventXmlEntity entity, TreeEvent model)
        {
            Collect(treeEvents, entity, model);
        }

        internal bool Contains(PersonXmlEntity entity)
        {
            return Contains(persons, entity);
        }

        internal bool Contains(FragilityCurveElementXmlEntity entity)
        {
            return Contains(fragilityCurveElements, entity);
        }

        internal bool Contains(EventTreeXmlEntity entity)
        {
            return Contains(eventTrees, entity);
        }

        internal bool Contains(TreeEventXmlEntity entity)
        {
            return Contains(treeEvents, entity);
        }

        internal Person Get(PersonXmlEntity entity)
        {
            return Get(persons, entity);
        }

        internal FragilityCurveElement Get(FragilityCurveElementXmlEntity entity)
        {
            return Get(fragilityCurveElements, entity);
        }

        internal EventTree Get(EventTreeXmlEntity entity)
        {
            return Get(eventTrees, entity);
        }

        internal TreeEvent Get(TreeEventXmlEntity entity)
        {
            return Get(treeEvents, entity);
        }

        public EventTree GetReferencedEventTree(long id)
        {
            var key = eventTrees.Keys.FirstOrDefault(k => k.Id == id);
            return key == null
                ? throw new ReadReferencedObjectsFirstException(nameof(EventTree))
                : Get(key);
        }

        public TreeEvent GetReferencedTreeEvent(long id)
        {
            var key = treeEvents.Keys.FirstOrDefault(k => k.Id == id);
            return key == null
                ? throw new ReadReferencedObjectsFirstException(nameof(TreeEvent))
                : Get(key);
        }

        #region helpers

        private TModel Get<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
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
                throw new ArgumentNullException(nameof(entity));

            return collection.ContainsKey(entity);
        }

        private void Collect<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TEntity entity, TModel model)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            collection[entity] = model;
        }

        private static Dictionary<TEntity, TModel> CreateDictionary<TEntity, TModel>()
        {
            return new Dictionary<TEntity, TModel>(new ReferenceEqualityComparer<TEntity>());
        }

        #endregion
    }
}