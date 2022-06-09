using System.Text.Json.Serialization;
using Todo.Core.Responses;

namespace Todo.Core.Dtos
{
    public class AuthenticateResponseDto : AuthenticateResponse
    {
        [JsonIgnore]
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonIgnore]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonIgnore]
        [JsonPropertyName("mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonIgnore]
        [JsonPropertyName("applicationUserName")]
        public string Name { get; set; }
        public BaseTokenDto Token { get; set; }

        public AuthenticateResponseDto(AuthenticateResponse response, string token)
        {
            Token = new BaseTokenDto { Token = token };
            Message = response.Message;
            Status = response.Status;
        }
    }
}
