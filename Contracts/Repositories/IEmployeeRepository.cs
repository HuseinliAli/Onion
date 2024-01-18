using Domain.Models;

namespace Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool changeTracker);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool changeTracker);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}