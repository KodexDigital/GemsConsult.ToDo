using System.Text.Json.Serialization;

namespace Todo.Core.Responses
{
    public class AuthenticateResponse
    {
        [JsonIgnore]
        public string Message { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
    }
}
