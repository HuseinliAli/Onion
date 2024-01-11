using Domain.Models;
using System.Collections.Generic;

namespace Contracts.Repositories;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool changeTracker);
    Company GetCompany(Guid id, bool changeTracker);
    void CreateCompany(Company company);
    IEnumerable<Company> GetByIds(IEnumerable<Guid> ids,bool changeTracker);    
}
