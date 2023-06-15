using ClientService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repository.Repositories.BaseRepository
{
    public class BaseRepository<TContext, TEntity> : IBaseRepository<TEntity> where TContext : DbContext where TEntity : class
    {
        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (disableTracking) query = query.AsNoTracking();

            if (expression != null) query = query.Where(expression);

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return Task.FromResult(query.AsQueryable());
        }

        public Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Task.FromResult(_dbContext.Set<TEntity>().Where(expression).AsQueryable());
        }

        public Task<IQueryable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> expression = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
            bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (disableTracking) query = query.AsNoTracking();

            if (expression != null) query = query.Where(expression);

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            

            return Task.FromResult(orderBy != null ? orderBy(query).AsQueryable() : query.AsQueryable());
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached) _dbContext.Set<TEntity>().Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;

            _dbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached) _dbContext.Set<TEntity>().Attach(entity);

            _dbContext.Set<TEntity>().Remove(entity);

            return Task.CompletedTask;
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public Task DeleteRange(IEnumerable<TEntity> entities)
        {
            var listEntities = entities.ToList();
            listEntities.ForEach(entity =>
            {
                if (_dbContext.Entry(entity).State == EntityState.Detached) _dbContext.Set<TEntity>().Attach(entity);
            });

            _dbContext.Set<TEntity>().RemoveRange(listEntities);

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(object id)
        {
            var entityToDelete = await _dbContext.Set<TEntity>().FindAsync(id);

            if (entityToDelete != null)
            {
                await DeleteAsync(entityToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public bool Any()
        {
            return _dbContext.Set<TEntity>().Any();
        }

        public async Task<BasePaginationEntity<TEntity>> PaginationAsync(int page = 0,
        int pageSize = 20,
        Expression<Func<TEntity, bool>> expression = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null,
        bool disableTracking = true)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (disableTracking) query = query.AsNoTracking();
            if (expression != null) query = query.Where(expression);

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var total = await query.CountAsync();

            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = await query.ToListAsync();

            return new BasePaginationEntity<TEntity>() { Data = data, Total = total };
        }
    }
}
