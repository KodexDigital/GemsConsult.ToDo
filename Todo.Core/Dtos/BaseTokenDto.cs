
using System.Text.Json.Serialization;

namespace Todo.Core.Dtos
{
    public class BaseTokenDto
    {
        [JsonPropertyName("tokenValue")]
        public string Token { get; set; }
    }
}
