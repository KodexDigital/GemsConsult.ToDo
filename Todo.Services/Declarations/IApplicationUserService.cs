using Todo.Core.Common;
using Todo.Core.Dtos;

namespace Todo.Services.Declarations
{
    public interface IApplicationUserService
    {
        Task<ResponseModel> AddNewUser(CreateNewUserDto dto);
        Task<ResponseModel<AuthenticateResponseDto>> LoginAsync(AuthenticationRequestDto model);
        Task<ResponseModel<List<AllApplicationUsersDto>>> GetAllUsers();
        Task<ApplicationUserDto> SingleUserDetailAsync(string userId);
    }
}