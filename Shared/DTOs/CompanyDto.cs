using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record CompanyDto() 
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    };
    public record EmployeeDto(Guid Id, string Name, int Age,string Position);
    public record EmployeeForCreationDto(string Name, int Age, string Position);
    public record CompanyForCreationDto(string Name, string Address, string Country,IEnumerable<EmployeeForCreationDto> Employees);
    
}
