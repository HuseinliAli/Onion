using AutoMapper;
using Contracts.Logging;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32.SafeHandles;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal sealed class AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration) : IAuthenticationService
    {
        private User? _user;
       
        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = mapper.Map<User>(userForRegistrationDto);

            var result = await userManager.CreateAsync(user, userForRegistrationDto.Password);

            if (result.Succeeded)
                await userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthDto userForAuth)
        {
            _user = await userManager.FindByNameAsync(userForAuth.UserName);

            var result = (_user!=null && await userManager.CheckPasswordAsync(_user, userForAuth.Password));

            if (!result)
                logger.LogWarning($"{nameof(ValidateUser)}: Auth failed. Wrong user name or password");

            return result;

        }
        public async Task<string> CreateToken()
        {
            var signInCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signInCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["secret"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret,SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, _user.UserName) };

            var roles = await userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
                List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
    }
}
