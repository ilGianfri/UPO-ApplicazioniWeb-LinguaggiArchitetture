using System;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class JwtToken
    {
        [JsonPropertyName("Token")]
        public string Token { get; set; }
        [JsonPropertyName("Expiration")]
        public DateTime Expiration { get; set; }
    }
}
