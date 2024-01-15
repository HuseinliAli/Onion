using Domain.Models;
using Shared.DTOs;

namespace Services.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool changeTracker);
    EmployeeDto GetEmployee(Guid companyId, Guid id, bool  changeTracker);
    EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool changeTracker);
    void DeleteEmployeeForCompany(Guid companyId, Guid id, bool changeTracker);
    void UpdateEmployeeForCompany(Guid companyId, Guid id,
        EmployeeForUpdateDto dto, bool compChangeTracker, bool empChangeTracker);
    (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(
        Guid companyId, Guid id, bool compChangeTracker, bool empChangeTracker);
    void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
}
