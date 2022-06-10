using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;
using Todo.DAL.Data;
using Todo.DAL.Repositories.Declarations;

namespace Todo.DAL.Repositories.Implementations
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly TodoDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<ApplicationUserRepository> logger;

        public ApplicationUserRepository(TodoDbContext context, UserManager<ApplicationUser> userManager,
             ILogger<ApplicationUserRepository> logger) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<ResponseModel> RegisterUser(CreateNewUserDto dto)
        {
            var response = new ResponseModel();
            logger.LogInformation($"{nameof(RegisterUser)} operation request");
            try
            {
                var userExits = await userManager.FindByNameAsync(dto.Email);
                if (userExits != null)
                {
                    response.Status = false;
                    response.Message = "User Already Exist";
                }

                ApplicationUser user = new()
                {
                    Name = dto.Name,
                    UserName = dto.Email,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    PasswordHash = dto.Password,
                };

                var result = await userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation($"{dto}");
                    response.Status = true;
                    response.Message = "User added";
                }
                else
                {
                    response.Status = result.Succeeded;
                    response.Message = "Error adding a user";
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception encountered => {ex}");
                throw new Exception(ex.Message.ToString());
            }
        }

        public async Task<ResponseModel<List<AllApplicationUsersDto>>> AllUsers()
        {
            var users = await context.Users.Select(u => new AllApplicationUsersDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Password = u.PasswordHash
            }).AsNoTracking().ToListAsync();

            return new ResponseModel<List<AllApplicationUsersDto>>
            {
                Status = users.Count > 0,
                Message = users.Count > 0 ? "users found" : "No record",
                Data = users
            };
        }

        public async Task<ApplicationUserDto> SingleUserDetail(string userId)
            => await context.Users.Where(u => u.Id.Equals(userId))
            .Select(u => new ApplicationUserDto
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Password = u.PasswordHash
            }).AsNoTracking().FirstOrDefaultAsync();
    }
}
