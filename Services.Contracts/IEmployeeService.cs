using Shared.DTOs;

namespace Services.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool changeTracker);
    EmployeeDto GetEmployee(Guid companyId, Guid id, bool  changeTracker);
}
