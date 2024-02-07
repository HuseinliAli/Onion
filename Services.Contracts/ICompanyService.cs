using Domain.Models;
using Entities.Responses;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts;
public interface ICompanyService
{
    ApiBaseResponse GetAllCompaniesAsync(bool changeTracker);
    ApiBaseResponse GetCompanyAsync(Guid companyId, bool changeTracker);
    Task<CompanyDto> CreateAsync(CompanyForCreationDto company);
    Task<IEnumerable<CompanyDto>> GetAllByIdsAsync(IEnumerable<Guid> ids, bool changeTracker);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection);
    Task DeleteCompanyAsync(Guid id, bool changeTracker);
    Task UpdateCompanyAsync(Guid companyId,CompanyForUpdateDto companyForUpdate, bool changeTracker);
}
