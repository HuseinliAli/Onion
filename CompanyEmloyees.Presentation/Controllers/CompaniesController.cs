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
        public IActionResult GetCompanies()
        {
           // throw new Exception("Exception");
            var companies = serviceManager.CompanyService.GetAllCompanies(false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}",Name ="CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = serviceManager.CompanyService.GetCompany(id,changeTracker: false);
            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
                return BadRequest("Company CreationDto object is null");
            var createdCompany = serviceManager.CompanyService.Create(company);
            return CreatedAtRoute("CompanyById", new {id = createdCompany.Id}, createdCompany);
        }
        [HttpGet("collection/{ids}",Name ="CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = serviceManager.CompanyService.GetAllByIds(ids, false);
            return Ok(companies);
        }
        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {
            var result = serviceManager.CompanyService.CreateCompanyCollection(companies);
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id)
        {
            serviceManager.CompanyService.DeleteCompany(id, false);
            return NoContent();
        }
    }
}
