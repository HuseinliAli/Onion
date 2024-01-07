using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController(IServiceManager service):BaseApiController
    {
        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var employees = service.EmployeeService.GetEmployees(companyId, false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee = service.EmployeeService.GetEmployee(companyId, id, changeTracker: false);
            return Ok(employee);
        }
    }
}
