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
    [Route("api/auth")]
    public class AuthenticationController(IServiceManager service):BaseApiController
    {
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto dto)
        {
            var result = await service.AuthenticationService.RegisterUser(dto);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Authenticate([FromBody] UserForAuthDto dto)
        {
            if (!await service.AuthenticationService.ValidateUser(dto))
                return Unauthorized();

            return Ok(new { Token = await service.AuthenticationService.CreateToken() });
        }
    }
}
