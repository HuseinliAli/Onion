using Domain.Models;
using Entities.LinkModels;
using Shared.DTOs;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Services.Contracts;

public interface IEmployeeService
{
    Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId,LinkParameters linkParameters, bool changeTracker);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool  changeTracker);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool changeTracker);
    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool changeTracker);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeForUpdateDto dto, bool compChangeTracker, bool empChangeTracker);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid id, bool compChangeTracker, bool empChangeTracker);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
}
