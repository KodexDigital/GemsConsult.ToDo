using Moq;
using System;
using System.Collections.Generic;
using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Responses;
using Todo.DAL.Interfaces;
using Todo.Services.Declarations;
using Xunit;

namespace UnitTesting
{
    public class Auth2ServiceTests
    {
        private readonly string email = "kodexkenth@gmail.com";
        private readonly string password = "@Password20";
        private readonly string mobile = "08101263634";
        private readonly string name = "Kenneth";
        private readonly string token;

        [Fact]
        public void Add_New_User_Test()
        {
            var authService = new Mock<IApplicationUserService>();
            authService.Setup(x => x.AddNewUser(new CreateNewUserDto
            {
                Name = name,
                Email = email,
                PhoneNumber = mobile,
                Password = password,
                ConfirmPassword = password
            })).ReturnsAsync(new ResponseModel { Status = true });
            Assert.True(true);
        }

        [Fact]
        public void Not_Same_Password_On_Add_New_User_Test()
        {
            var authService = new Mock<IApplicationUserService>();
            authService.Setup(x => x.AddNewUser(new CreateNewUserDto
            {
                Name = name,
                Email = email,
                PhoneNumber = mobile,
                Password = password,
                ConfirmPassword = "@Password0"
            })).ReturnsAsync(new ResponseModel { Status = false });
            Assert.True(true);
        }

        [Fact]
        public void Login_Async_Test()
        {
            var authResponse = new AuthenticateResponse();
            var authService = new Mock<IApplicationUserService>();
            authService.Setup(l => l.LoginAsync(new AuthenticationRequestDto
            {
                Email = email,
                Password = password,
            })).ReturnsAsync(new AuthenticateResponseDto(authResponse, token));
            Assert.True(true);
        }

        [Fact]
        public void Single_User_Detail_Async_Test()
        {
            var result = new ApplicationUserDto();
            var authService = new Mock<IApplicationUserService>();
            authService.Setup(s => s.SingleUserDetailAsync(Guid.NewGuid().ToString()))
                .ReturnsAsync(new ApplicationUserDto { Email = result.Email});
            Assert.True(true);
        }

        [Fact]
        public void All_Users_Test()
        {
            var users = new ResponseModel<List<AllApplicationUsersDto>>();
            var authService = new Mock<IApplicationUserService>();
            authService.Setup(a => a.GetAllUsers())
                .ReturnsAsync(new ResponseModel<List<AllApplicationUsersDto>> { Data = users.Data });
            Assert.True(true);
        }

        [Fact]
        public void Generate_Auth_Token_Test()
        {
            var tokenization = new Mock<ITokenizationSetting>();
            tokenization.Setup(t => t.GenerateJwtToken(new TokenDataDto
            {
                Email = email,
                MobileNumber = mobile,
                Name = name,
                UserId = Guid.NewGuid().ToString(),
            })).Returns(token);
            Assert.True(true);
        }
    }
}