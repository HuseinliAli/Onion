using Microsoft.AspNetCore.JsonPatch;
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
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController(IServiceManager service) : BaseApiController
    {

        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = service.EmployeeService.GetEmployees(companyId, false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = service.EmployeeService.GetEmployee(companyId, id, changeTracker: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("Employee for Creation object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var employeeToReturn = service.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);

            return CreatedAtRoute("GetEmployeeForCompany", new
            {companyId,id =employeeToReturn.Id},employeeToReturn);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId,Guid id)
        {
            service.EmployeeService.DeleteEmployeeForCompany(companyId, id,false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("Employee for update dto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            service.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee,false, true);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null");

            var result = service.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeToPatch,ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);
            return NoContent();
        }
    }

}
