using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Repositories.BaseRepository
{
    public class BaseRepository<TContext, TEntity> : IBaseRepository<TEntity> where TContext : DbContext where TEntity : class
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public TEntity? Find(int id)
        {
            return _dbSet.Find(id);
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if(includeFunc != null)
            {
                query = includeFunc(query);
            }    

            return query.FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if(expression != null)
            {
                query = query.Where(expression);
            }

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return query.ToList();
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public (int, IEnumerable<TEntity>) Pagination(int page = 0,
        int pageSize = 20,
        Expression<Func<TEntity, bool>>? expression = null, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            var total = query.Count();

            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = query.ToList();

            return (total, data);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
