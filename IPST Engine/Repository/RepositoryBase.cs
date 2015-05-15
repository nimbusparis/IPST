using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Util;

namespace IPST_Engine.Repository
{

    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected ISession _session;

        public TEntity Get(TKey key)
        {
            return _session.Get<TEntity>(key);
        }

        public IList<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return _session.QueryOver<TEntity>().Where(expression).List<TEntity>();
        }

        public IList<TEntity> GetAll()
        {
            return _session.QueryOver<TEntity>().List<TEntity>();
        }

        public void Save(TEntity entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Save(IList<TEntity> entities)
        {
            using (var transaction = _session.BeginTransaction())
            {
                entities.ForEach(e=>_session.SaveOrUpdate(e));
                transaction.Commit();
            }
        }
    }
}