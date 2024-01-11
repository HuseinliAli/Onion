using Contracts.Repositories;
using Domain.Models;
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

    public Employee GetEmployee(Guid companyId, Guid id, bool changeTracker)
        => FindCondition(x=>x.CompanyId.Equals(companyId ) && x.Id.Equals(id),changeTracker)
           .SingleOrDefault();

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool changeTracker)
        =>FindCondition(x=>x.CompanyId.Equals(companyId),changeTracker)
        .OrderBy(x=>x.Name).ToList();
}
