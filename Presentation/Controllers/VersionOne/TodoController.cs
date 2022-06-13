using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;
using Todo.Services.Declarations;

namespace Presentation.Controllers.VersionOne
{
    [ApiVersion("1.0")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    public class TodoController : BaseController
    {
        private readonly ITodoService todoService;

        public TodoController(ITodoService todoService)
        {
            this.todoService = todoService;
        }

        /// <summary>
        /// Create a new item of action
        /// </summary>
        /// <param name="request">Required payload request</param>
        /// <returns>Success expected</returns>
        [HttpPost, Route("addItem")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateTodoItem([FromBody] TodoDto request)
            => Ok(await todoService.CreateTodoItem(request));

        /// <summary>
        /// Update excuted item
        /// </summary>
        /// <param name="itemId">Request payload</param>
        /// <returns>Success expected</returns>
        [HttpPut, Route("updateExcutedItem/{itemId}")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ExecuteedItemUpdate(int itemId)
            => Ok(await todoService.ExecuteedItemUpdate(itemId));

        /// <summary>
        /// The endpoint is used for editing an item
        /// </summary>
        /// <param name="request">Request payload</param>
        /// <returns>Expected success</returns>
        [HttpPut, Route("editItem")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> EditItem([FromBody] EditTodoDto request)
            => Ok(await todoService.EditItem(request));

        /// <summary>
        /// The is the endpoint to remove an item
        /// </summary>
        /// <param name="itemId">Request payload</param>
        /// <returns>Success expected</returns>
        [HttpPost, Route("removeItem/{itemId}")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RemoveItem(int itemId)
            => Ok(await todoService.RemoveItem(itemId));

        /// <summary>
        /// Get all items created
        /// </summary>
        /// <returns>List of the items</returns>
        [HttpGet, Route("allTodoItems")]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<TodoEntity>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<TodoEntity>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllTodos()
           => Ok(await todoService.GetAllTodos());

        /// <summary>
        /// Get items based on a user
        /// </summary>
        /// <param name="userId">Request payload</param>
        /// <returns>List of the items</returns>
        [HttpGet, Route("userTodoItems/{userId}")]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<TodoEntity>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<TodoEntity>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllUserTodos(string userId)
           => Ok(await todoService.AllUserTodoItems(userId));

        /// <summary>
        /// This endpont get a single item from the database
        /// </summary>
        /// <param name="itemId">Request payload</param>
        /// <returns>Single record</returns>
        [HttpGet, Route("getItem/{itemId}")]
        [ProducesResponseType(typeof(ResponseModel<TodoEntity>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<TodoEntity>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSingleTodos(int itemId)
           => Ok(await todoService.GetSingleItem(itemId));
    }
}
