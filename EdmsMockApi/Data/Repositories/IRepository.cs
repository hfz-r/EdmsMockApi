using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdmsMockApi.Entities;

namespace EdmsMockApi.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }

        Task DeleteAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        Task InsertAsync(IEnumerable<TEntity> entities);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
    }
}