using System;
using System.Text.Json.Serialization;

namespace JobScheduler.Models
{
    public class JwtToken
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
    }
}
