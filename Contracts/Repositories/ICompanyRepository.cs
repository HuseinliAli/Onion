using Domain.Models;
using System.Collections.Generic;

namespace Contracts.Repositories;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool changeTracker);
    Task<Company> GetCompanyAsync(Guid id, bool changeTracker);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids,bool changeTracker);
    void DeleteCompany(Company company);
}
