using Microsoft.AspNetCore.Identity;

namespace JobScheduler.Shared.Models
{
    public class UserWithRole
    {
        public IdentityUser User { get; set; }
        public string Role { get; set; }

        public UserWithRole()
        {
            User = new IdentityUser();
            Role = string.Empty;
        }
    }
}
