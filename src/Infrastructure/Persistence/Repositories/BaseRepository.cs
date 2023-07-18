using Application.Paging;
using Application.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public IQueryable<T> Query() => _context.Set<T>();

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            entity.CreateDate = DateTime.UtcNow;
            _context.Add(entity);
        }

        public void Delete(T entity)
        {
            entity.Deleted = true;
            entity.UpdateDate = DateTime.UtcNow;
            _context.Update(entity);
        }

        public Task<T> Get(int id, CancellationToken cancellationToken)
        {
            return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IPaginate<T>> GetList(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, int index = 0, int size = 10, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = Query();
            if(!enableTracking)
                queryable = queryable.AsNoTracking();
            if(include != null)
                queryable = include(queryable);
            if(predicate != null)
                queryable = queryable.Where(predicate);
            if (orderBy != null)
                return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);

            return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
        }

        public void Update(T entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            _context.Update(entity);
        }
    }
}
