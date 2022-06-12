using Todo.Core.Common;
using Todo.Core.Entities;

namespace Todo.DAL.Repositories.Declarations
{
    public interface ITodoRepository : IGenericRepository<TodoEntity>
    {
        Task<ResponseModel<List<TodoEntity>>> AllTodoItems();
        Task<ResponseModel<List<TodoEntity>>> AllUserTodoItems(string userId);
    }
}
