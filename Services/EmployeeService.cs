using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Entities.Exceptions;
using Services.Contracts;
using Shared.DTOs;

namespace Services;

internal sealed class EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IEmployeeService
{
    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool changeTracker)
    {
        var company = repositoryManager.Company.GetCompany(companyId, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeFromDb = repositoryManager.Employee.GetEmployee(companyId, id, changeTracker);
        if (employeeFromDb is null)
            throw new EmployeeNotFoundException(companyId); 
        var employeeDto = mapper.Map<EmployeeDto>(employeeFromDb);
        return employeeDto;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool changeTracker)
    {
        var company = repositoryManager.Company.GetCompany(companyId,changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeesFromDb = repositoryManager.Employee.GetEmployees(companyId, changeTracker);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        return employeesDto;
    }
}
