using ClientService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.Repositories.BaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
            bool disableTracking = true);

        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? expression = null);

        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
            bool disableTracking = true);

        Task<TEntity?> GetByIdAsync(object id);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        Task DeleteRange(IEnumerable<TEntity> entities);

        Task DeleteAsync(object id);

        bool Any();

        public Task<BasePaginationEntity<TEntity>> PaginationAsync(int page = 0,
        int pageSize = 20,
        Expression<Func<TEntity, bool>> expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
        bool disableTracking = true);
    }
}
