﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JobScheduler.Models
{
    public class UserWithRole
    {
        public IdentityUser User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
