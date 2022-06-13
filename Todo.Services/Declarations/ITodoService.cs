using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;

namespace Todo.Services.Declarations
{
    public interface ITodoService
    {
        Task<ResponseModel> CreateTodoItem(TodoDto dto);
        Task<ResponseModel<List<TodoEntity>>> GetAllTodos();
        Task<ResponseModel<List<TodoEntity>>> AllUserTodoItems(string userId);
        Task<ResponseModel> ExecuteedItemUpdate(int itemId);
        Task<ResponseModel<TodoEntity>> GetSingleItem(int itemId);
        Task<ResponseModel> RemoveItem(int itemId);
        Task<ResponseModel> EditItem(EditTodoDto dto);
    }
}