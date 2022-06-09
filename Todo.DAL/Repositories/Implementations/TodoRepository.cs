using Todo.Core.Entities;
using Todo.DAL.Data;
using Todo.DAL.Repositories.Declarations;

namespace Todo.DAL.Repositories.Implementations
{
    public class TodoRepository : GenericRepository<TodoEntity>, ITodoRepository
    {
        private readonly TodoDbContext context;

        public TodoRepository(TodoDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
