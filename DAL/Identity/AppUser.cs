using System;
using Microsoft.AspNetCore.Identity;

namespace DAL.Identity
{
    public class AppUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}

