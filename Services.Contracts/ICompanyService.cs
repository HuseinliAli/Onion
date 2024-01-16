using Domain.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts;
public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool changeTracker);
    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool changeTracker);
    Task<CompanyDto> CreateAsync(CompanyForCreationDto company);
    Task<IEnumerable<CompanyDto>> GetAllByIdsAsync(IEnumerable<Guid> ids, bool changeTracker);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection);
    Task DeleteCompanyAsync(Guid id, bool changeTracker);
    Task UpdateCompanyAsync(Guid companyId,CompanyForUpdateDto companyForUpdate, bool changeTracker);
}
