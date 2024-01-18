using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contexts;
using Repositories.Extensions;
using Shared.RequestFeatures;

namespace Repositories.Repos;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee)
        =>Delete(employee);

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool changeTracker)
        =>await FindCondition(x=>x.CompanyId.Equals(companyId ) && x.Id.Equals(id),changeTracker)
           .SingleOrDefaultAsync();

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters ,bool changeTracker)
    {
        var employees = await FindCondition(x => x.CompanyId.Equals(companyId), changeTracker)
        .FilterEmployees(employeeParameters.MinAge,employeeParameters.MaxAge)
        .Search(employeeParameters.SearchTerm)
        .OrderBy(x => x.Name)
        .Skip((employeeParameters.PageNumber-1)*employeeParameters.PageSize)
        .Take(employeeParameters.PageSize)
        .ToListAsync();
        
        var count = await FindCondition(x => x.CompanyId.Equals(companyId), changeTracker).CountAsync();

        return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
    }
}
