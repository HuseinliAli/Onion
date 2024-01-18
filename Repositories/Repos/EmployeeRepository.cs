using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contexts;

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

    public  async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool changeTracker)
        =>await FindCondition(x=>x.CompanyId.Equals(companyId),changeTracker)
        .OrderBy(x=>x.Name).ToListAsync();
}
