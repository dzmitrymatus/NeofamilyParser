using System.Linq.Expressions;

namespace NeofamilyParser.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filterExpression);
        Task<TEntity?> GetAsync(object id);
        Task<TEntity?> GetRandomAsync();
        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAllAsync();
    }
}
