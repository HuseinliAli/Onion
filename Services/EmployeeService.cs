using AutoMapper;
using Contracts.Hateoas;
using Contracts.Logging;
using Contracts.Managers;
using Contracts.Shapers;
using Domain.Models;
using Entities.Exceptions;
using Entities.LinkModels;
using Services.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System.ComponentModel.Design;
using System.Dynamic;

namespace Services;

internal sealed class EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper,IDataShaper<EmployeeDto> dataShaper,IEmployeeLinks employeeLinks) : IEmployeeService
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

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId,LinkParameters linkParameters, bool changeTracker)
    {
        if (!linkParameters.EmployeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExistsAsync(companyId, changeTracker);

        var employeesWithMetaData =await repositoryManager.Employee
            .GetEmployeesAsync(companyId,linkParameters.EmployeeParameters, changeTracker);

        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        var links = employeeLinks.TryGenerateLinks(employeesDto, linkParameters.EmployeeParameters.Fields, companyId, linkParameters.Context);
       
        return (links,metaData:employeesWithMetaData.MetaData);
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
