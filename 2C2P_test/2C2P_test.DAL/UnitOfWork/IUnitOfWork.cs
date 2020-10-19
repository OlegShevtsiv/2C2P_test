using _2C2P_test.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        int SaveChanges();
    }
}
