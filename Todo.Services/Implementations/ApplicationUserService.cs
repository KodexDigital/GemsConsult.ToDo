using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;
using Todo.Core.Responses;
using Todo.DAL.Interfaces;
using Todo.DAL.Repositories.Declarations;
using Todo.Logger.Servicer.Manager;
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
        private readonly ILoggerManager logger;
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration config;
        private readonly string defaultName = nameof(ApplicationUserService);

        public ApplicationUserService(IApplicationUserRepository userRepository, ITokenizationSetting tokenization, IUnitOfWork uow,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILoggerManager logger,
            IMemoryCache memoryCache, IConfiguration config)
        {
            this.userRepository = userRepository;
            this.tokenization = tokenization;
            this.uow = uow;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.config = config;
        }
        public async Task<ResponseModel> AddNewUser(CreateNewUserDto dto)
        {
            string location = string.Concat(defaultName, "<-----",nameof(AddNewUser));
            try
            {
                var response = await userRepository.RegisterUser(dto);
                if (response.Status.Equals(true))
                {
                    await uow.ExecuteCommandAsync();

                    logger.LogInfo($"{location} ::: {response}");
                    return new ResponseModel
                    {
                        Status = response.Status,
                        Message = response.Message
                    };
                }

                logger.LogInfo($"{location} ::: {response}");
                return new ResponseModel
                {
                    Status = response.Status,
                    Message = response.Message
                };
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }

        public async Task<ResponseModel<List<AllApplicationUsersDto>>> GetAllUsers()
        {
            string location = string.Concat(defaultName, "<-----", nameof(GetAllUsers));
            try
            {
                var cacheKey = config.GetSection("InMemoryCache:CacheKey").Value;
                //chccking cache if it exists
                if (!memoryCache.TryGetValue(cacheKey, out ResponseModel<List<AllApplicationUsersDto>> users))
                {
                    users = await userRepository.AllUsers();
                    //setting cache options
                    var cacheExpiration = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    };

                    //setting cache entries
                    memoryCache.Set(cacheKey, users, cacheExpiration);

                }
                //var users = await userRepository.AllUsers();
                logger.LogInfo(users.Data.Count > 0 ? $"{location} ::: {users.Data.Count} User(s) found" : $"{location} ::: No record found");
                return users;
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }
        public async Task<ResponseModel<AuthenticateResponseDto>> LoginAsync(AuthenticationRequestDto model)
        {
            string location = string.Concat(defaultName, "<-----", nameof(LoginAsync));
            try
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

                        logger.LogInfo($"{location} ::: {authResponse.Status + "<>" + authResponse.Message}");
                    }
                    else
                    {
                        authResponse.Status = false;
                        authResponse.Message = "Login failed";

                        logger.LogInfo($"{location} ::: {authResponse.Status +"<>"+ authResponse.Message}");
                    }

                    return new ResponseModel<AuthenticateResponseDto>
                    {
                        Status = authResponse.Status,
                        Message = authResponse.Message,
                        Data = new AuthenticateResponseDto(authResponse, token)
                    };
                }

                return new ResponseModel<AuthenticateResponseDto>
                {
                    Status = false,
                    Message = "Login Failed",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }

        public async Task<ApplicationUserDto> SingleUserDetailAsync(string userId)
        {
            string location = string.Concat(defaultName, "<-----", nameof(SingleUserDetailAsync));
            try
            {
                var user = await userRepository.SingleUserDetail(userId);
                logger.LogInfo($"{location} ::: User found");
                return user;
            }
            catch (Exception ex)
            {
                logger.LogInfo($"{location} ::: {ex.Message}");
                throw;
            }
        }

    }
}
