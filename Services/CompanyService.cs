﻿using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Domain.Models;
using Entities.Exceptions;
using Entities.Responses;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        var company =await GetCompanyAndCheckIfItExistsAsync(id, changeTracker);
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

    public ApiBaseResponse GetAllCompaniesAsync(bool changeTracker)
    {
        var companies = repositoryManager.Company.GetAllCompaniesAsync(changeTracker);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return new ApiOkResponse<IEnumerable<CompanyDto>>(companiesDto);
    }

    public ApiBaseResponse GetCompanyAsync(Guid companyId, bool changeTracker)
    {
        var company = GetCompanyAndCheckIfItExistsAsync(companyId, changeTracker);
        var companyDto = mapper.Map<CompanyDto>(company);
        return new ApiOkResponse<CompanyDto>(companyDto);
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool changeTracker)
    {
        var company =await GetCompanyAndCheckIfItExistsAsync(companyId, changeTracker);
        mapper.Map(companyForUpdate, company);
        await repositoryManager.SaveAsync();
    }

    private async Task<Company> GetCompanyAndCheckIfItExistsAsync(Guid id, bool changeTracker)
    {
        var company = await repositoryManager.Company.GetCompanyAsync(id, changeTracker);
        if (company is null)
            throw new CompanyNotFoundException(id);
        return company;
    }
}
