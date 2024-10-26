using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Vis.VleadProcessV3.Models;

namespace Vis.VleadProcessV3.Repositories
{
    public class QueryBuilder<TEntity> : IQueryBuilder<TEntity> where TEntity : class
    {
        private ApplicationDbContext context;
        private TallyDbContext Tally;
        private IQueryable<TEntity> query;
        private IQueryable<TEntity> querytally;
        private IQueryable inque;
        private IEnumerable<TEntity> ery;
        private DbSet<TEntity> dbSet;
        private DbSet<TEntity> dbSettally;
       
        public QueryBuilder(ApplicationDbContext context,TallyDbContext Tally)
        {
            this.context = context;
            query = this.context.Set<TEntity>();
            dbSet = this.context.Set<TEntity>();
            ery = this.context.Set<TEntity>();
            inque = this.context.Set<TEntity>();
            this.Tally = Tally;
            this.querytally = this.Tally.Set<TEntity>();
            this.dbSettally = this.Tally.Set<TEntity>();
        }
        //public QueryBuilder(TallyDbContext Tally)
        //{
        //    this.Tally = Tally;
        //    this.querytally = this.Tally.Set<TEntity>();
        //    this.dbSettally = this.Tally.Set<TEntity>();
        //}

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return this.query.Where(predicate);
        }
        //
        public Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.query.Where(predicate).ToListAsync();
        }
        //
        public IQueryBuilder<TEntity> Include(string path)
        {
            this.query = this.query.Include(path);
            return this;
        }
        public IQueryBuilder<TEntity> Include(Expression<Func<TEntity, bool>> predicate)
        {
            this.query = this.query.Include(predicate);
            return this;
        }

        public IOrderedEnumerable<TEntity> OrderBy(Func<TEntity, int> KeySelector)
        {
            return this.ery.OrderBy(KeySelector);
        }
        public IOrderedEnumerable<TEntity> OrderBy(Func<TEntity, object> KeySelector)
        {
            return this.ery.OrderBy(KeySelector);
        }
        public IOrderedEnumerable<TEntity> OrderBy(Func<TEntity, bool> KeySelector)
        {
            return this.ery.OrderBy(KeySelector);
        }

        public IOrderedEnumerable<TEntity> OrderByDescending(Func<TEntity, int> KeySelector)
        {
            return this.ery.OrderByDescending(KeySelector);
        }
        public IOrderedEnumerable<TEntity> OrderByDescending(Func<TEntity, object> KeySelector)
        {
            return this.ery.OrderByDescending(KeySelector);
        }
        public IOrderedEnumerable<TEntity> OrderByDescending(Func<TEntity, bool> KeySelector)
        {
            return this.ery.OrderByDescending(KeySelector);
        }

        public IQueryable<TEntity> Page(int page, int pageSize)
        {
            return this.query.Skip(page * pageSize).Take(pageSize);
        }

        public TEntity FirstOrDefault()
        {
            return this.query.FirstOrDefault<TEntity>();
        }
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.query.FirstOrDefault<TEntity>(predicate);
        }
        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return this.querytally.FirstOrDefault<TEntity>(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync()
        {
            return this.query.FirstOrDefaultAsync();
        }

        public List<TEntity> ToList()
        {
            return this.ery.ToList();
        }

        public Task<List<TEntity>> ToListAsync()
        {
            return this.query.ToListAsync();
        }

        public int Count()
        {
            return this.query.Count();
        }
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.query.Count(predicate);
        }

        public Task<int> CountAsync()
        {
            return this.query.CountAsync();
        }


        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }

        //For tally db use only
        public virtual void Insert(TEntity entity)
        {
            dbSettally.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        public bool Any(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            var set = this.context.Set<TEntity>();
            return (predicate == null) ? set.Any() : set.Any(predicate);
        }
        public virtual void Update(TEntity entityToUpdate)
        {
            // dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual TEntity Find(object id)
        {
            return dbSet.Find(id);
        }
        public virtual LocalView<TEntity> Local()
        {
         return this.dbSet.Local;
        }
        public virtual void SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            context.Database.SqlQueryRaw<TEntity>(sql, parameters);
        }

        
    }
}
