using CompanyEmloyees.Presentation.ModelBinders;
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
    [Route("api/companies")]
    public class CompaniesController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
           // throw new Exception("Exception");
            var companies =await serviceManager.CompanyService.GetAllCompaniesAsync(false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}",Name ="CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company =await serviceManager.CompanyService.GetCompanyAsync(id,changeTracker: false);
            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("Company CreationDto object is null");
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
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody]CompanyForUpdateDto company)
        {
            if (company is null)
                return BadRequest("Company for update dto object is null");
            await serviceManager.CompanyService.UpdateCompanyAsync(id, company, true);
            return NoContent(); 
        }
    }
}
