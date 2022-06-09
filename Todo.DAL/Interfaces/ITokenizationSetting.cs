
using Todo.Core.Dtos;

namespace Todo.DAL.Interfaces
{
    public interface ITokenizationSetting
    {
        string GenerateJwtToken(TokenDataDto tokenData);
    }
}
