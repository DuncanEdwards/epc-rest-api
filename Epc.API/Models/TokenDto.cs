using Newtonsoft.Json;

namespace Epc.API.Models
{
    public class TokenDto
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; } = "Bearer";

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
    
    }
}
