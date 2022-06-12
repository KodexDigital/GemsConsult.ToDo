
using System.Text.Json.Serialization;

namespace Todo.Core.Settings
{
    public class JwtSettings
    {
        [JsonPropertyName("authKey")]
        public string AuthKey { get; set; }

        [JsonPropertyName("authIssuer")]
        public string AuthIssuer { get; set; }

        [JsonPropertyName("authAudience")]
        public string AuthAudience { get; set; }
    }
}
