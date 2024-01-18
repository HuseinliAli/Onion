using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Domain.Models;
using Entities.Exceptions;
using Services.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System.ComponentModel.Design;

namespace Services;

internal sealed class EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : IEmployeeService
{
    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool changeTracker)
    {
        await CheckIfCompanyExistsAsync(companyId, changeTracker);

        var employeeEntity = mapper.Map<Employee>(employeeForCreationDto);   
        repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);

        await repositoryManager.SaveAsync();
        var employeeToReturn = mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool changeTracker)
    {
        await CheckIfCompanyExistsAsync(companyId, changeTracker);

        var employeeForCompany = await GetEmployeeForCompanyIfItExists(companyId, id, changeTracker);

        repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        await repositoryManager.SaveAsync();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool changeTracker)
    {
        await CheckIfCompanyExistsAsync(companyId, changeTracker);

        var employeeFromDb = await GetEmployeeForCompanyIfItExists(companyId, id, changeTracker);

        var employeeDto = mapper.Map<EmployeeDto>(employeeFromDb);
        return employeeDto;
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compChangeTracker, bool empChangeTracker)
    {
        await CheckIfCompanyExistsAsync(companyId, compChangeTracker);

        var employeeEntity = await GetEmployeeForCompanyIfItExists(companyId, id, compChangeTracker);

        var employeeToPatch = mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId,EmployeeParameters employeeParameters, bool changeTracker)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExistsAsync(companyId, changeTracker);

        var employeesWithMetaData =await repositoryManager.Employee
            .GetEmployeesAsync(companyId,employeeParameters, changeTracker);

        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

        return (employees:employeesDto,metaData:employeesWithMetaData.MetaData);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        mapper.Map(employeeToPatch, employeeEntity);
        await repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto dto, bool compChangeTracker, bool empChangeTracker)
    {
        await CheckIfCompanyExistsAsync(companyId, compChangeTracker);

        var employeeEntity = await GetEmployeeForCompanyIfItExists(companyId, id, empChangeTracker);

        mapper.Map(dto, employeeEntity);

        await repositoryManager.SaveAsync();
    }

    private async Task CheckIfCompanyExistsAsync(Guid id, bool changeTracker)
    {
        var company =await repositoryManager.Company.GetCompanyAsync(id, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(id);
    }
    private async Task<Employee> GetEmployeeForCompanyIfItExists(Guid companyId, Guid id, bool changeTracker)
    {
        var employeeEntity = await repositoryManager.Employee.GetEmployeeAsync(companyId, id, changeTracker);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(companyId);
        return employeeEntity;
    }
}
