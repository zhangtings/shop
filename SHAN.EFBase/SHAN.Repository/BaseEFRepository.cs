using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using SHAN.Entity;

namespace SHAN.Repository
{
    
    public class BaseEFRepository<TEntity> : IEFRepository<TEntity> where TEntity : class
    {
        public EFDbContext _dbContext;

        public BaseEFRepository(EFDbContext efDbContext)
        {
            this._dbContext = efDbContext;
        }

        public IQueryable<TEntity> Entities => _dbContext.Set<TEntity>();

        public int Delete(object id, bool isSave)
        {
            var model = _dbContext.Set<TEntity>().Find(id);
            _dbContext.Set<TEntity>().Remove(model);
            return _dbContext.SaveChanges();
        }

        public int Delete(TEntity entity, bool isSave)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(IEnumerable<TEntity> entities, bool isSave)
        {
            foreach (var i in entities)
            {
                _dbContext.Set<TEntity>().Remove(i);
            }
            return _dbContext.SaveChanges();
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity GetById(object key)
        {
            return _dbContext.Set<TEntity>().Find(key);
        }

        public int Insert(TEntity entity, bool isSave)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Insert(IEnumerable<TEntity> entities, bool isSave)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
            return _dbContext.SaveChanges();
        }

        public int Update(TEntity entity, bool isSave)
        {
            _dbContext.Set<TEntity>().AddOrUpdate(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(IEnumerable<TEntity> entities, bool isSave)
        {
            _dbContext.Set<TEntity>().AddOrUpdate(entities.ToArray());
            return _dbContext.SaveChanges();
        }

        public int Update(Expression<Func<TEntity, object>> propertyExpression, TEntity entity, bool isSave)
        {
            throw new NotImplementedException();
        }
    }
}
