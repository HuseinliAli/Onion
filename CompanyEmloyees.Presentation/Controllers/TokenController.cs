using CompanyEmloyees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Controllers
{
    [Route("api/token")]
    public class TokenController(IServiceManager service):BaseApiController
    {
        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto dto)
        {
            var tokenToReturn = await service.AuthenticationService.RefreshToken(dto);

            return Ok(tokenToReturn);   
        }
    }
}
