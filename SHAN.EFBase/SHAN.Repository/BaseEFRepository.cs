using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        private DbSet<TEntity> _entities;

        protected virtual DbSet<TEntity> Table
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<TEntity>();

                return _entities;
            }
        }

        public virtual IQueryable<TEntity> 实体集 => Table;

        public IQueryable<TEntity> Entities => _dbContext.Set<TEntity>();

        public IQueryable<TEntity> AsNoTracking => Entities.AsNoTracking();

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

        //public int Update(TEntity entity, bool isSave)
        //{
        //    _dbContext.Set<TEntity>().AddOrUpdate(entity);
        //    return _dbContext.SaveChanges();
        //}


        public int Update(TEntity entity, bool isSave)
        {
            DbSet<TEntity> dbSet = _dbContext.Set<TEntity>();
            DbEntityEntry<TEntity> entry = _dbContext.Entry(entity);
            try
            {

                if (entry.State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
            catch (InvalidOperationException)
            {
                PropertyInfo key = entity.GetType().GetProperties().FirstOrDefault(r => r.GetCustomAttributes(typeof(KeyAttribute), false) != null);
                TEntity oldEntity = dbSet.Find(key.GetValue(entity));
                var xx = _dbContext.Entry(oldEntity).State;
                _dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
                xx = _dbContext.Entry(oldEntity).State;
                foreach (PropertyInfo p in oldEntity.GetType().GetProperties())
                {
                    var diyi = _dbContext.Entry<TEntity>(oldEntity);
                    var dier = diyi.Property(p.Name);
                    var disan = dier.IsModified;
                    if (p.Name == "Addtime" || p.GetValue(oldEntity) == null)
                    {
                        dier.IsModified = false;
                    }
                }
            }

            //var set = _dbContext.Set<TEntity>();
            //set.AddOrUpdate(entity);
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
