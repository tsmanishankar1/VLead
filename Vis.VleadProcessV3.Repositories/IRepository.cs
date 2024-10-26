using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Vis.VleadProcessV3.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {

        LocalView<TEntity> Local() ; 
        int Count(Expression<Func<TEntity, bool>> filter);

        //IQueryable<TResult> Join<TInner, TKey, TResult>(IRepository<TInner> innerRepository, Expression<Func<TEntity, TKey>> outerSelector, Expression<Func<TInner, TKey>> innerSelector, Expression<Func<TEntity, TInner, TResult>> resultSelector) where TInner : class;

        IQueryable<TEntity> GetAllVal(params Expression<Func<TEntity, object>>[] includeExpressions);

       

        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeExpressions);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);

        TEntity GetByID(object id);

        TEntity GetByString(string val1, string val2);

        void Insert(TEntity entity);

        void InsertCollection(IEnumerable<TEntity> entites);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        bool Exist(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null);

        void AddRange(IEnumerable<TEntity> entities);

        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity GetLastRecord(Expression<Func<TEntity, object>> expression);
    }
}
