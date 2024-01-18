using CompanyEmloyees.Presentation.ActionFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController(IServiceManager service) : BaseApiController
    {

        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult =await service.EmployeeService.GetEmployeesAsync(companyId, employeeParameters,false);
            
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee =await service.EmployeeService.GetEmployeeAsync(companyId, id, changeTracker: false);
            return Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var employeeToReturn =await service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);

            return CreatedAtRoute("GetEmployeeForCompany", new
            {companyId,id =employeeToReturn.Id},employeeToReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId,Guid id)
        {
            await service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id,false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]EmployeeForUpdateDto employee)
        {
            await service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee,false, true);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null");

            var result =await service.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeToPatch,ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
            return NoContent();
        }
    }

}
