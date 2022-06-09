using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;

namespace Todo.Services.Declarations
{
    public interface ITodoService
    {
        Task<ResponseModel> CreateTodoItem(TodoDto dto);
        Task<ResponseModel<IEnumerable<TodoEntity>>> GetAllTodos();
        Task<ResponseModel> ExecuteedItemUpdate(int itemId);
    }
}
