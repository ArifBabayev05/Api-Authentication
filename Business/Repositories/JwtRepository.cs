using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Services;
using DAL.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Repositories
{
    public class JwtRepository : IJwtService
    {

        private readonly IConfiguration _config;
        public JwtRepository(IConfiguration config)
        {
            _config = config;
        }

        public string GetToken(AppUser user, IList<string> roles)
        {
            var issuer = _config.GetSection("JWT:issuer").Value;
            var audience = _config.GetSection("JWT:audience").Value;
            var secretKey = _config.GetSection("JWT:secretKey").Value;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Firstname", user.Firstname),
                new Claim("Lastname", user.Lastname)
            };

            //var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(n => new Claim(ClaimTypes.Role, n)));

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                audience: audience,
                issuer: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4).AddMinutes(15),
                signingCredentials: signingCredentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);


            return token;

        }
    }
}

