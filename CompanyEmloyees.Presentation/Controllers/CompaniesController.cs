using Application.Commands;
using Application.Notifications;
using Application.Queries;
using CompanyEmloyees.Presentation.ActionFilters;
using Marvin.Cache.Headers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;


namespace CompanyEmloyees.Presentation.Controllers
{
    [ApiVersion("1.0")]
    //[Route("api/{v:apiversion}/companies
    [Route("api/companies")]
    [ResponseCache(CacheProfileName = "120SecondsDuration")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController(ISender sender, IPublisher publisher) : BaseApiController
    {
        /// <summary>
        /// Gets the list of all companies
        /// </summary>
        /// <returns>The companies list</returns>
        [HttpGet("GetCompanies")]
        //[Authorize(Roles ="Adminstrator")]
        public async Task<IActionResult> GetCompanies()
        {
            // throw new Exception("Exception");
            var companies = await sender.Send(new GetCompaniesQuery(false));
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await sender.Send(new GetCompanyQuery(id, false));
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
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreationDto)
        {
            if (companyForCreationDto is null)
                return BadRequest("Company for creation dto is null");
            var company = await sender.Send(new CreateCompanyCommand(companyForCreationDto));

            return CreatedAtRoute("CompanyById", new { id = company.Id }, company);
        }
        //[HttpGet("collection/{ids}",Name ="CompanyCollection")]
        //public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        //{
        //    var companies =await serviceManager.CompanyService.GetAllByIdsAsync(ids, false);
        //    return Ok(companies);
        //}
        //[HttpPost("collection")]
        //public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        //{
        //    var result =await serviceManager.CompanyService.CreateCompanyCollectionAsync(companies);
        //    return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        //}

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await publisher.Publish(new CompanyDeleteNotification(id, false));
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if (company is null)
                return BadRequest("CompanyForUpdateDto object is null");

            await sender.Send(new UpdateCompanyCommand(id, company, true));

            return NoContent();
        }
    }
}
