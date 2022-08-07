using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Commons;
using Business.DTO.Auth;
using DAL.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private  readonly  RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser appUser = new AppUser();
            appUser.Firstname = registerDto.Firstname;
            appUser.Lastname = registerDto.Lastname;
            appUser.Email = registerDto.Email;
            appUser.UserName = registerDto.Email;

            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            string error = "";

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    error += item.Description + "\n";
                }

                return StatusCode(StatusCodes.Status401Unauthorized, new Response(4563, error));
            }    
                
                
            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response(4563, "Email Does Not Exist"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response(4563, "Invalid password"));

            }

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

            var roles = await _userManager.GetRolesAsync(user);

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


            
                
           

            return Ok(token); 
        }
        
    }
}