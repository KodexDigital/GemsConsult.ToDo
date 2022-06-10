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
    }
}
