using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contexts;

namespace Repositories.Repos;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public void CreateCompany(Company company)
        => Create(company);

    public void DeleteCompany(Company company)
        => Delete(company);

    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool changeTracker)
        =>await FindAll(changeTracker).OrderBy(c=>c.Name).ToListAsync();

    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool changeTracker)
        =>await FindCondition(x => ids.Contains(x.Id), changeTracker).ToListAsync();

    public async Task<Company> GetCompanyAsync(Guid id, bool changeTracker)
        =>await FindCondition(x=>x.Id.Equals(id),changeTracker).SingleOrDefaultAsync();
}
