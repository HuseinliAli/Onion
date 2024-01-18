using Domain.Models;
using Shared.RequestFeatures;
namespace Contracts.Repositories;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,EmployeeParameters employeeParameters,bool changeTracker);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool changeTracker);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}