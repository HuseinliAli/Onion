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
    public CompanyDto Create(CompanyForCreationDto company)
    {
        var copmanyEntity = mapper.Map<Company>(company);
        repositoryManager.Company.CreateCompany(copmanyEntity);
        repositoryManager.Save();
        var companyToReturn = mapper.Map<CompanyDto>(copmanyEntity);
        return companyToReturn;
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
            throw new CompanyCollectionBadRequest();
        var companyEntites = mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var company in companyEntites)
        {
            repositoryManager.Company.CreateCompany(company);
        }
        repositoryManager.Save();
        var companyCollectionToReturn = 
            mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
        var ids = string.Join(",", companyCollectionToReturn.Select(x => x.Id));
        return (companyCollectionToReturn, ids);            
    }

    public void DeleteCompany(Guid id, bool changeTracker)
    {
        var company = repositoryManager.Company.GetCompany(id,changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(id);
        repositoryManager.Company.DeleteCompany(company);
        repositoryManager.Save();
    }

    public IEnumerable<CompanyDto> GetAllByIds(IEnumerable<Guid> ids, bool changeTracker)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();
        var companyEntites = repositoryManager.Company.GetByIds(ids, changeTracker);
        if (ids.Count() != companyEntites.Count())
            throw new CollectionByIdsBadRequestException();
        var companiesToReturn = mapper.Map<IEnumerable<CompanyDto>>(companyEntites);
        return companiesToReturn;
    }

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

    public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool changeTracker)
    {
        var companyEntity = repositoryManager.Company.GetCompany(companyId, changeTracker);
        if (companyEntity is null)
            throw new CompanyNotFoundException(companyId);
        mapper.Map(companyForUpdate, companyEntity);
        repositoryManager.Save();

    }
}
