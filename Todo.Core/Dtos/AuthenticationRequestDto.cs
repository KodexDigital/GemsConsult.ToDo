using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.Core.Dtos
{
    public class AuthenticationRequestDto
    {
        [JsonPropertyName("email")]
        [Required]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }
    }
}
