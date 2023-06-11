using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.Repositories.BaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity);
        public void Remove(TEntity entity);
        public TEntity Find(int id);
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
        public (int, IEnumerable<TEntity>) Pagination(int page = 0,
        int pageSize = 20,
        Expression<Func<TEntity, bool>>? expression = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);

        public void Update(TEntity entity);
    }
}
