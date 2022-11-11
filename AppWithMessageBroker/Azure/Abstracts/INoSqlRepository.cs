using System.Linq.Expressions;

namespace Azure.Abstracts
{
    public interface INoSqlRepository<TEntity>
    {
        TEntity Find(Expression<Func<TEntity, bool>> filter);
        TEntity Find(string partitionKey, string rowKey);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null);
        bool Add(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(string partitionKey, string rowKey);
    }
}
