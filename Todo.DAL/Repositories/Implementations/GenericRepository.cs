using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Todo.DAL.Data;
using Todo.DAL.Repositories.Declarations;

namespace Todo.DAL.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly TodoDbContext context;
        protected internal readonly DbSet<TEntity> dbSet;

        public GenericRepository(TodoDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public async Task<IEnumerable<TEntity>> GetAsync() => await dbSet.ToListAsync();
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => await dbSet.FirstOrDefaultAsync(predicate);
        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate)
           => await dbSet.AnyAsync(predicate);
    }
}
