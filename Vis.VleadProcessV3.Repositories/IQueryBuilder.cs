using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Vis.VleadProcessV3.Repositories
{
    public interface IQueryBuilder<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryBuilder<TEntity> Include(string path);
        IQueryBuilder<TEntity> Include(Expression<Func<TEntity, bool>> predicate);
        IOrderedEnumerable<TEntity> OrderBy(Func<TEntity, int> KeySelector);
        IOrderedEnumerable<TEntity> OrderBy(Func<TEntity, object> KeySelector);
        IOrderedEnumerable<TEntity> OrderBy(Func<TEntity, bool> KeySelector);
        IOrderedEnumerable<TEntity> OrderByDescending(Func<TEntity, int> KeySelector);
        IOrderedEnumerable<TEntity> OrderByDescending(Func<TEntity, object> KeySelector);
        IOrderedEnumerable<TEntity> OrderByDescending(Func<TEntity, bool> KeySelector);
        IQueryable<TEntity> Page(int page, int pageSize);

        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync();

        List<TEntity> ToList();
        Task<List<TEntity>> ToListAsync();

        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Insert(TEntity entity);
        void Delete(object id);
        void RemoveRange(IEnumerable<TEntity> entity);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        TEntity Find(object id);
        LocalView<TEntity> Local();

        void SqlQuery<TEntity>(string sql, params object[] parameters);

        bool Any(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null);
    }
}
