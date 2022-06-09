using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Core.Entities;

namespace Todo.DAL.Repositories.Declarations
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ResponseModel> RegisterUser(CreateNewUserDto dto);
        Task<ResponseModel<List<AllApplicationUsersDto>>> AllUsers();
        Task<ApplicationUserDto> SingleUserDetail(string userId);
    }
}
