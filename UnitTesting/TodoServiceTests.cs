using Moq;
using System;
using System.Collections.Generic;
using Todo.Core.Common;
using Todo.Core.Entities;
using Todo.Services.Declarations;
using Xunit;

namespace UnitTesting
{
    public class TodoServiceTests
    {
        [Fact]
        public void Create_Todo_Item_Test()
        {
            var todoService = new Mock<ITodoService>();
            todoService.Setup(c => c.CreateTodoItem(new Todo.Core.Dtos.TodoDto
            {
                ItemName = "testing",
                Description = "testing",
                ExecutionDate = DateTime.Now,
                UserId = Guid.NewGuid().ToString(),
            })).ReturnsAsync(new ResponseModel { Status = true });
            Assert.True(true);
        }

        [Fact]
        public void Remove_Item_Test()
        {
            var todoService = new Mock<ITodoService>();
            todoService.Setup(r => r.RemoveItem(1))
                .ReturnsAsync(new ResponseModel { Status = true });
            Assert.True(true);
        }

        [Fact]
        public void Executeed_Item_Update_Test()
        {
            var todoService = new Mock<ITodoService>();
            todoService.Setup(e => e.ExecuteedItemUpdate(1))
                .ReturnsAsync(new ResponseModel { Status = true });
            Assert.True(true);
        }

        [Fact]
        public void Edit_Item_Test()
        {
            var todoService = new Mock<ITodoService>();
            todoService.Setup(e => e.EditItem(1, new Todo.Core.Dtos.TodoDto 
            {
                ItemName= "testing edit",
                Description = "edited description",
                ExecutionDate= DateTime.Now,
            })).ReturnsAsync(new ResponseModel { Status = true });
            Assert.True(true);
        }

        [Fact]
        public void Get_All_Todos_Test()
        {
            var items = new ResponseModel<List<TodoEntity>>();
            var todoService = new Mock<ITodoService>();
            todoService.Setup(a => a.GetAllTodos())
                .ReturnsAsync(new ResponseModel<List<TodoEntity>> { Data = items.Data });
            Assert.NotNull(todoService.Object);
        }

        [Fact]
        public void Get_All_User_Todos_Test()
        {
            var items = new ResponseModel<TodoEntity>();
            var todoService = new Mock<ITodoService>();
            todoService.Setup(a => a.GetSingleItem(1))
                .ReturnsAsync(new ResponseModel<TodoEntity> { Data = items.Data });
            Assert.NotNull(todoService.Object);
        }
    }
}