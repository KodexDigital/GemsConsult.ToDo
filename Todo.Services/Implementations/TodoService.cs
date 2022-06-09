using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;
using Todo.DAL.Repositories.Declarations;
using Todo.Logger.Servicer.Manager;
using Todo.Services.Declarations;

namespace Todo.Services.Implementations
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository repo;
        private readonly IUnitOfWork uow;
        private readonly ILoggerManager logger;

        public TodoService(ITodoRepository repo, IUnitOfWork uow, ILoggerManager logger)
        {
            this.repo = repo;
            this.uow = uow;
            this.logger = logger;
        }
        public async Task<ResponseModel> CreateTodoItem(TodoDto dto)
        {
            var response = new ResponseModel();
            var itemExists = await uow.TodoRepository.ExistAsync(c => c.ItemName.ToLower().Equals(dto.ItemName.ToLower()));
            if (itemExists)
            {
                response.Status = false;
                response.Message = "Item already exists";

                logger.LogInfo($"{response}");
            }

            repo.Add(new TodoEntity
            {
                ItemName = dto.ItemName,
                Description = dto.Description,
                ExecutionDate = dto.ExecutionDate,
            });

            var result = await uow.ExecuteCommandAsync();

            if (result > 0)
            {
                response.Status = true;
                response.Message = $"Item added successfully";

                logger.LogInfo($"{response}");
            }
            else
            {
                response.Status = false;
                response.Message = "Item was not successful";

                logger.LogInfo($"{response}");
            }

            logger.LogInfo($"{response}");
            return response;
        }

        public async Task<ResponseModel> ExecuteedItemUpdate(int itemId)
        {
            var response = new ResponseModel();
            var item = await uow.TodoRepository.GetAsync(i => i.TodoId.Equals(itemId));
            if(item != null)
            {
                item.IsExecuted = true;
                await uow.ExecuteCommandAsync();

                response.Status = true;
                response.Message = "Item excuted";

                logger.LogInfo($"{response}");
            }
            else
            {
                response.Status = false;
                response.Message = "Item not not found";

                logger.LogInfo($"{response}");
            }

            return response;
        }
        public async Task<ResponseModel<IEnumerable<TodoEntity>>> GetAllTodos()
        {
            var list = await uow.TodoRepository.GetAsync();
            logger.LogInfo(list.Count() > 0 ? $"{list.Count()} Item(s) found" : "No record found");
            return new ResponseModel<IEnumerable<TodoEntity>>
            {
                Status = list.Count() > 0,
                Data = list.OrderByDescending(l => l.ExecutionDate),
                Message = list.Count() > 0 ? $"{list.Count()} Item(s) found" : "No record found"
            };
        }
    }
}
