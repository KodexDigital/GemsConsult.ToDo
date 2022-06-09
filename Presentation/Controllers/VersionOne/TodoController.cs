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
        public async Task<IActionResult> OnboardCustomer([FromBody] TodoDto request)
            => Ok(await todoService.CreateTodoItem(request));

        /// <summary>
        /// Update excuted item
        /// </summary>
        /// <param name="itemId">Request payload</param>
        /// <returns>Success expected</returns>
        [HttpPut, Route("updateExcutedItem/{itemId}")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> OnboardCustomer(int itemId)
            => Ok(await todoService.ExecuteedItemUpdate(itemId));

        /// <summary>
        /// Get all items created
        /// </summary>
        /// <returns>List of the items</returns>
        [HttpGet, Route("allTodoItems")]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<TodoEntity>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<IEnumerable<TodoEntity>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AllExistingCustomers()
           => Ok(await todoService.GetAllTodos());
    }
}
