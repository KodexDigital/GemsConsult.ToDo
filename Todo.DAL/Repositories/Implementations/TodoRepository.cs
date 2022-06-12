using Microsoft.EntityFrameworkCore;
using Todo.Core.Common;
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

        public async Task<ResponseModel<List<TodoEntity>>> AllTodoItems()
        {
            return new ResponseModel<List<TodoEntity>>
            {
                Data = await context.Todos.Where(t => !t.Remove).AsNoTracking().ToListAsync()
            };
        }
        public async Task<ResponseModel<List<TodoEntity>>> AllUserTodoItems(string userId)
        {
            return new ResponseModel<List<TodoEntity>>
            {
                Data = await context.Todos.Where(u => u.ApplicationUserId == userId)
                .Where(t => !t.Remove).AsNoTracking().ToListAsync()
            };
        }
    }
}
