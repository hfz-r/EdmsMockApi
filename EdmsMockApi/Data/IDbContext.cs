using System.Threading;
using System.Threading.Tasks;
using EdmsMockApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace EdmsMockApi.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}