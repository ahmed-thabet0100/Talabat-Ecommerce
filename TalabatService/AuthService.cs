using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repo.Contarct;

namespace TalabatService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration Config)
        {
            _config = Config;
        }
        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            // private claims
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var token = new JwtSecurityToken(
                audience: _config["Jwt:ValidAudience"],
                issuer: _config["Jwt:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:DurationInDays"])),
                claims: Claims,
                signingCredentials: new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)
                );

            return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
