using Domain.Models;

namespace Contracts.Repositories;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool changeTracker);
}
