using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthDto userForAuth);
        Task<TokenDto> CreateToken(bool populateExp);

        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
