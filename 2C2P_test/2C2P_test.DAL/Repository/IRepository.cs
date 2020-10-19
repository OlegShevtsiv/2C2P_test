﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace _2C2P_test.DAL.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void Remove(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
    }
}
