using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Domain.Models;
using Entities.Exceptions;
using Services.Contracts;
using Shared.DTOs;

namespace Services;

internal sealed class EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IEmployeeService
{
    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool changeTracker)
    {
        var company = repositoryManager.Company.GetCompany(companyId, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeEntity = mapper.Map<Employee>(employeeForCreationDto);   
        repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        repositoryManager.Save();
        var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool changeTracker)
    {
        var company = repositoryManager.Company.GetCompany(companyId, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeForCompany = repositoryManager.Employee.GetEmployee(companyId, id, changeTracker);
        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);
        repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        repositoryManager.Save();
    }

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
