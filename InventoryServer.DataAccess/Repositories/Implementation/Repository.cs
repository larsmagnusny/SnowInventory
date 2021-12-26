using InventoryServer.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace InventoryServer.DataAccess.Repositories.Implementation
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly IDbConnection db;

        public Repository(IDbConnection _db)
        {
            db = _db;
        }

        public virtual TEntity Get(TKey key)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
