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
    IEnumerable<CompanyDto> GetAllCompanies(bool changeTracker);
    CompanyDto GetCompany(Guid companyId, bool changeTracker);
    CompanyDto Create(CompanyForCreationDto company);
    IEnumerable<CompanyDto> GetAllByIds(IEnumerable<Guid> ids, bool changeTracker);
    (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection
        (IEnumerable<CompanyForCreationDto> companyCollection);
    void DeleteCompany(Guid id, bool changeTracker);
