using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class UserWithRole
    {
        [JsonPropertyName("User")]
        public IdentityUser User { get; set; }
        [JsonPropertyName("Role")]
        public string Role { get; set; }
    }
}
