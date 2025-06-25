using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NeofamilyParser.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> DbSet { get; private set; }
        protected DbContext DbContext { get; private set; }

        public Repository(NeofamilyParserDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filterExpression)
        {
            if (filterExpression != null)
            {
                return await filterExpression
                    .Compile()
                    .Invoke(DbSet)
                    .ToListAsync()
                    .ContinueWith(x => (IEnumerable<TEntity>)x.Result);
            }

            return await DbSet
                .ToListAsync()
                .ContinueWith(x => (IEnumerable<TEntity>)x);
        }

        public async Task<TEntity?> GetAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity?> GetRandomAsync()
        {
            return await DbSet.OrderBy(x => Guid.NewGuid())
                .FirstAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            var entityType = DbContext.Model.FindEntityType(typeof(TEntity)).GetTableName();
            await DbSet.AddRangeAsync(entities);
            await DbContext.Database.OpenConnectionAsync();
            await DbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{entityType} ON");           
            await DbContext.SaveChangesAsync();
            await DbContext.Database.CloseConnectionAsync();
        }
         
        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var result = DbSet.Remove(entity);
            if (result.State == EntityState.Deleted)
            {
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAllAsync()
        {
            await DbSet.ExecuteDeleteAsync();
        }
    }
}
