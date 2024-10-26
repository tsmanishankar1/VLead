using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Repositories
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
        {
            internal ApplicationDbContext context;
            internal DbSet<TEntity> dbSet;

        //public Repository()
        //{

        //}
            public Repository(ApplicationDbContext context)
            {
                this.context = context;
                dbSet = context.Set<TEntity>();
               
            
            }
        public virtual LocalView<TEntity> LocalVal(TEntity entity)
        {
            return this.dbSet.Local;
        }
        public virtual LocalView<TEntity> Local()
        {
            return this.dbSet.Local;
        }
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.AsNoTracking().Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.AsNoTracking().Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return orderBy(query);
                }
                else
                {
                    return query;
                }
            }

            public virtual TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
            {
                TEntity item = null;
                {
                    IQueryable<TEntity> dbQuery = context.Set<TEntity>();

                    foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

                    item = dbQuery
                        .AsNoTracking()
                        .FirstOrDefault(where);
                }
                return item;
            }

            public virtual TEntity GetByString(string val1, string val2)
            {
                return dbSet.Find(val1, val2);
            }


            public virtual TEntity GetByID(object id)
            {
                return dbSet.Find(id);
            }

            public virtual void Insert(TEntity entity)
            {
                dbSet.Add(entity);
            }

            public virtual void InsertCollection(IEnumerable<TEntity> entites)
            {
                dbSet.AddRange(entites);
            }

            public virtual void Delete(object id)
            {
                TEntity entityToDelete = dbSet.Find(id);
                Delete(entityToDelete);
            }

            public virtual void Delete(TEntity entityToDelete)
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            }

            public virtual void Update(TEntity entityToUpdate)
            {
                //dbSet.Attach(entityToUpdate);
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }

            public bool Exist(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
            {
                var set = this.context.Set<TEntity>();
                return (predicate == null) ? set.Any() : set.Any(predicate);
            }
         
            public virtual IQueryable<TEntity> GetAll(
            params Expression<Func<TEntity, object>>[] includeExpressions)
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                foreach (var includeExpression in includeExpressions)
                {
                    dbSet = dbSet.Include(includeExpression);
                }
                return dbSet;
            }

            public virtual IQueryable<TEntity> GetAllVal(params Expression<Func<TEntity, object>>[] includeExpressions)
            {

                IQueryable<TEntity> dbSet = context.Set<TEntity>();
           
                return includeExpressions
                  .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>
                   (dbSet, (current, expression) => current.Include(expression));
           
            }

            public int Count(Expression<Func<TEntity, bool>> filter)
            {
                IQueryable<TEntity> dbSet = context.Set<TEntity>();

                return dbSet.Where(filter).Count();
            }
            public virtual void AddRange(IEnumerable<TEntity> entities)
            {
                context.Set<TEntity>().AddRange(entities);
            }

            public virtual void RemoveRange(IEnumerable<TEntity> entities)
            {
                context.Set<TEntity>().RemoveRange(entities);
            }
        public virtual TEntity GetLastRecord(Expression<Func<TEntity, object>> expression)
        {
           return context.Set<TEntity>().OrderBy(expression).LastOrDefault();
        }
        }
    }

