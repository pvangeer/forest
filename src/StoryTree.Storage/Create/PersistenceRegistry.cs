using System;
using System.Collections.Generic;
using System.Linq;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage.Create
{
    public class PersistenceRegistry
    {
        private readonly Dictionary<ProjectEntity, Project> projects = CreateDictionary<ProjectEntity, Project>();
        private readonly Dictionary<ExpertEntity, Expert> experts = CreateDictionary<ExpertEntity, Expert>();
        private readonly Dictionary<PersonEntity, Person> persons = CreateDictionary<PersonEntity, Person>();

        #region Register Methods

        internal void Register(ProjectEntity entity, Project model)
        {
            Register(projects, entity, model);
        }

        internal void Register(ExpertEntity entity, Expert model)
        {
            Register(experts, entity, model);
        }
        internal void Register(PersonEntity entity, Person model)
        {
            Register(persons, entity, model);
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
        #endregion

        #region helpers
        private static Dictionary<TEntity, TModel> CreateDictionary<TEntity, TModel>()
        {
            return new Dictionary<TEntity, TModel>(new ReferenceEqualityComparer<TEntity>());
        }

        private bool ContainsValue<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return collection.Values.Contains(model, new ReferenceEqualityComparer<TModel>());
        }

        private void Register<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TEntity entity, TModel model)
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

        private TEntity Get<TEntity, TModel>(Dictionary<TEntity, TModel> collection, TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return collection.Keys.Single(k => ReferenceEquals(collection[k], model));
        }
        #endregion
    }
}