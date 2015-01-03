using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IPST_Engine.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TEntity Get(TKey key);
        IList<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        IList<TEntity> GetAll();
        void Save(TEntity entity);
        void Save(IList<TEntity> entities);
    }
}