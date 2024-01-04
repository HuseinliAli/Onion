using Contracts.Repositories;
using Domain.Models;
using Repositories.Contexts;

namespace Repositories.Repos;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
