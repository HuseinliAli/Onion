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
    public async Task<CompanyDto> CreateAsync(CompanyForCreationDto company)
    {
        var copmanyEntity = mapper.Map<Company>(company);
        repositoryManager.Company.CreateCompany(copmanyEntity);
        await repositoryManager.SaveAsync();
        var companyToReturn = mapper.Map<CompanyDto>(copmanyEntity);
        return companyToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();
        var companyEntites = mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntites)
        {
            repositoryManager.Company.CreateCompany(company);
        }
        await repositoryManager.SaveAsync();
        var companyCollectionToReturn = 
            mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
        var ids = string.Join(",", companyCollectionToReturn.Select(x => x.Id));
        return (companyCollectionToReturn, ids);            
    }

    public async Task DeleteCompanyAsync(Guid id, bool changeTracker)
    {
        var company =await repositoryManager.Company.GetCompanyAsync(id,changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(id);
        repositoryManager.Company.DeleteCompany(company);
        await repositoryManager.SaveAsync();
    }

    public async Task<IEnumerable<CompanyDto>> GetAllByIdsAsync(IEnumerable<Guid> ids, bool changeTracker)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();
        var companyEntites =await repositoryManager.Company.GetByIdsAsync(ids, changeTracker);
        if (ids.Count() != companyEntites.Count())
            throw new CollectionByIdsBadRequestException();
        var companiesToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
        return companiesToReturn;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool changeTracker)
    {
        var companies =await repositoryManager.Company.GetAllCompaniesAsync(changeTracker);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool changeTracker)
    {
        var company =await repositoryManager.Company.GetCompanyAsync(companyId, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool changeTracker)
    {
        var companyEntity =await repositoryManager.Company.GetCompanyAsync(companyId, changeTracker);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);
        mapper.Map(companyForUpdate, companyEntity);
        await repositoryManager.SaveAsync();
    }
}
