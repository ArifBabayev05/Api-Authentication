using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Identity;

namespace Business.Services
{
    public interface IJwtService
    {
        string GetToken(AppUser user, List<string> roles);
    }
}

