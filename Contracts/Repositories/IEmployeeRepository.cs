using Domain.Models;

namespace Contracts.Repositories;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetEmployees(Guid companyId, bool changeTracker);
    Employee GetEmployee(Guid companyId, Guid id, bool changeTracker);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
}