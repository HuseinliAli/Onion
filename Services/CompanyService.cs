using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Domain.Models;
using Entities.Exceptions;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
internal sealed class CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper) : ICompanyService
{
    public IEnumerable<CompanyDto> GetAllCompanies(bool changeTracker)
    {
        var companies = repositoryManager.Company.GetAllCompanies(changeTracker);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public CompanyDto GetCompany(Guid companyId, bool changeTracker)
    {
        var company = repositoryManager.Company.GetCompany(companyId, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }
}
