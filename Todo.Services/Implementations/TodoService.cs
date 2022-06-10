using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration config;
        private readonly string defaultName = nameof(TodoService);

        public TodoService(ITodoRepository repo, IUnitOfWork uow, ILoggerManager logger, 
            IMemoryCache memoryCache, IConfiguration config)
        {
            this.repo = repo;
            this.uow = uow;
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.config = config;
        }
        public async Task<ResponseModel> CreateTodoItem(TodoDto dto)
        {
            string location = string.Concat(defaultName, "<-----", nameof(CreateTodoItem));
            try
            {
                var response = new ResponseModel();
                var itemExists = await uow.TodoRepository.ExistAsync(c => c.ItemName.ToLower().Equals(dto.ItemName.ToLower()));
                if (itemExists)
                {
                    response.Status = false;
                    response.Message = "Item already exists";
                }

                repo.Add(new TodoEntity
                {
                    ItemName = dto.ItemName,
                    Description = dto.Description,
                    ExecutionDate = dto.ExecutionDate,
                    ApplicationUserId = dto.UserId
                });

                var result = await uow.ExecuteCommandAsync();

                if (result > 0)
                {
                    response.Status = true;
                    response.Message = $"Item added successfully";
                }
                else
                {
                    response.Status = false;
                    response.Message = "Item was not successful";
                }

                logger.LogInfo($"{location} ::: {response}");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }

        public async Task<ResponseModel> ExecuteedItemUpdate(int itemId)
        {
            string location = string.Concat(defaultName, "<-----", nameof(ExecuteedItemUpdate));
            try
            {
                var response = new ResponseModel();
                var item = await uow.TodoRepository.GetAsync(i => i.TodoId.Equals(itemId));
                if (item != null)
                {
                    if (!item.Remove)
                    {
                        item.IsExecuted = true;
                        await uow.ExecuteCommandAsync();

                        response.Status = true;
                        response.Message = "Item excuted";
                    }

                    response.Status = false;
                    response.Message = "Item has been removed";
                }
                else
                {
                    response.Status = false;
                    response.Message = "Item not not found"; 
                }

                logger.LogInfo($"{location} ::: {response}");
                return response;
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }
        public async Task<ResponseModel<List<TodoEntity>>> GetAllTodos()
        {
            string location = string.Concat(defaultName, "<-----", nameof(GetAllTodos));
            try
            {
                var cacheKey = config.GetSection("InMemoryCache:CacheKey").Value;
                if (!memoryCache.TryGetValue(cacheKey, out ResponseModel<List<TodoEntity>> list))
                {
                    list = await uow.TodoRepository.AllTodoItems();
                    var cacheExpiration = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(5),
                    };

                    memoryCache.Set(cacheKey, list, cacheExpiration);
                }

                logger.LogInfo(list.Data.Count() > 0 ? $"{location} ::: {list.Data.Count()} Item(s) found" : $"{location} ::: No record found");
                return new ResponseModel<List<TodoEntity>>
                {
                    Status = list.Data.Count() > 0,
                    Data = list.Data,
                    Message = list.Data.Count() > 0 ? $"{list.Data.Count()} Item(s) found" : "No record found"
                };
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }

        public async Task<ResponseModel> RemoveItem(int itemId)
        {
            string location = string.Concat(defaultName, "<-----", nameof(RemoveItem));
            try
            {
                var item = await uow.TodoRepository.GetAsync(t => t.TodoId.Equals(itemId));
                if (item != null)
                {
                    item.Remove = true;
                    await uow.ExecuteCommandAsync();

                    logger.LogInfo($"{location} ::: Item removed successfully");
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Item removed successfully"
                    };
                }

                logger.LogInfo($"{location} ::: Error removing item");
                return new ResponseModel
                {
                    Status = false,
                    Message = "Error removing item"
                };
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }
    }
}
