using _2C2P_test.DAL.EF;
using _2C2P_test.DAL.Models;
using _2C2P_test.DAL.Repository;
using _2C2P_test.DAL.Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private BankDbContext db;
        private TransactionRepository TransactionRepository;

        public IRepository<Transaction> Transaction 
        {
            get
            {
                if (TransactionRepository == null)
                {
                    TransactionRepository = new TransactionRepository(db);
                }
                return TransactionRepository;
            }
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        #region Disposing
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
