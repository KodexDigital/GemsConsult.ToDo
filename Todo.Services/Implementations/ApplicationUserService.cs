using Microsoft.AspNetCore.Identity;
using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;
using Todo.Core.Responses;
using Todo.DAL.Interfaces;
using Todo.DAL.Repositories.Declarations;
using Todo.Services.Declarations;

namespace Todo.Services.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository userRepository;
        private readonly ITokenizationSetting tokenization;
        private readonly IUnitOfWork uow;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ApplicationUserService(IApplicationUserRepository userRepository, ITokenizationSetting tokenization, IUnitOfWork uow,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userRepository = userRepository;
            this.tokenization = tokenization;
            this.uow = uow;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<ResponseModel> AddNewUser(CreateNewUserDto dto)
        {
            var response = await userRepository.RegisterUser(dto);
            if (response.Equals(true))
            {
                await uow.ExecuteCommandAsync();
                return new ResponseModel
                {
                    Status = response.Status,
                    Message = response.Message
                };
            }

            return new ResponseModel
            {
                Status = response.Status,
                Message = response.Message
            };
        }

        public async Task<ResponseModel<List<AllApplicationUsersDto>>> GetAllUsers()
            => await userRepository.AllUsers();
        public async Task<AuthenticateResponseDto> LoginAsync(AuthenticationRequestDto model)
        {
            var tokenData = new TokenDataDto();
            string token = string.Empty;

            var authResponse = new AuthenticateResponse();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email,
                           model.Password, false, false);

                if (result.Succeeded)
                {
                    tokenData.UserId = user.Id;
                    tokenData.Email = user.Email;
                    tokenData.MobileNumber = user.PhoneNumber;
                    tokenData.Name = user.Name;

                    token = tokenization.GenerateJwtToken(tokenData);

                    authResponse.Status = true;
                    authResponse.Message = "Login successful";
                }
                else
                {
                    authResponse.Status = false;
                    authResponse.Message = "Login failed";
                }
            }

            return new AuthenticateResponseDto(authResponse, token);
        }

        public async Task<ApplicationUserDto> SingleUserDetailAsync(string userId)
            => await userRepository.SingleUserDetail(userId);

    }
}
