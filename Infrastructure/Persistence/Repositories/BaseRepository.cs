using Application.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

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

        public Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return _context.Set<T>().ToListAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            _context.Update(entity);
        }
    }
}
