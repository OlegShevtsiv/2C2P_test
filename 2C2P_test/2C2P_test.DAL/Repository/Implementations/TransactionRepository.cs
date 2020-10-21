using _2C2P_test.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using _2C2P_test.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace _2C2P_test.DAL.Repository.Implementations
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private BankDbContext db;

        public TransactionRepository(BankDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Transaction> Get(Func<Transaction, bool> predicate)
        {
            var result = db.Transactions.AsEnumerable().Where(predicate);
            return result;
        }

        public void Add(Transaction entity)
        {
            db.Transactions.Add(entity);
        }

        public void Add(IEnumerable<Transaction> entities)
        {
            db.Transactions.AddRange(entities);
        }

        public void Remove(Transaction entity)
        {
            Transaction entityToDelete = db.Transactions.Find(entity.Id);
            if (entityToDelete != null)
            {
                db.Transactions.Remove(entityToDelete);
            }
        }

        public void Remove(IEnumerable<Transaction> entities)
        {
            Transaction entityToDelete;
            foreach (var e in entities) 
            {
                entityToDelete = db.Transactions.Find(e.Id);
                if (entityToDelete != null)
                {
                    db.Transactions.Remove(entityToDelete);
                }
            }
        }

        public void Update(Transaction entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
    }
}
