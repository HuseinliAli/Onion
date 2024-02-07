using CompanyEmloyees.Presentation.ActionFilters;
using CompanyEmloyees.Presentation.Extensions;
using CompanyEmloyees.Presentation.ModelBinders;
using Entities.Responses;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Controllers
{
    [ApiVersion("1.0")]
    //[Route("api/{v:apiversion}/companies
    [Route("api/companies")]
    [ResponseCache(CacheProfileName ="120SecondsDuration")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController(IServiceManager serviceManager) : BaseApiController
    {
        /// <summary>
        /// Gets the list of all companies
        /// </summary>
        /// <returns>The companies list</returns>
        [HttpGet("GetCompanies")]
        [Authorize(Roles ="Adminstrator")]
        public IActionResult GetCompanies()
        {
            // throw new Exception("Exception");
            var baseResult = serviceManager.CompanyService.GetAllCompaniesAsync(false);
            var companies = baseResult.GetResult<IEnumerable<CompanyDto>>();
            return Ok(companies);
        }

        [HttpGet("{id:guid}",Name ="CompanyById")]
        [HttpCacheExpiration(CacheLocation=CacheLocation.Public,MaxAge =60)]
        [HttpCacheValidation(MustRevalidate =false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var baseResult = serviceManager.CompanyService.GetCompanyAsync(id, false);
            if (!baseResult.Success)
                return ProcessError(baseResult);

            var company = baseResult.GetResult<CompanyDto>();
            return Ok(company);
        }

        /// <summary>
        /// Creates a newly created company
        /// </summary>
        /// <param name="company"></param>
        /// <returns>A newly created company</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("CreateCompany")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany =await serviceManager.CompanyService.CreateAsync(company);
            return CreatedAtRoute("CompanyById", new {id = createdCompany.Id}, createdCompany);
        }
        [HttpGet("collection/{ids}",Name ="CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies =await serviceManager.CompanyService.GetAllByIdsAsync(ids, false);
            return Ok(companies);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {
            var result =await serviceManager.CompanyService.CreateCompanyCollectionAsync(companies);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await serviceManager.CompanyService.DeleteCompanyAsync(id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody]CompanyForUpdateDto company)
        {
            await serviceManager.CompanyService.UpdateCompanyAsync(id, company, true);
            return NoContent(); 
        }
    }
}
