using Application.Commands;
using Application.Queries;
using AutoMapper;
using Contracts.Managers;
using Domain.Models;
using Entities.Exceptions;
using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers;
internal sealed class GetCompaniesHandler(IRepositoryManager repository,IMapper mapper) : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
{
    public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await repository.Company.GetAllCompaniesAsync(request.ChangeTracker);
        var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;    
    }
}

internal sealed class GetCompanyHandler(IRepositoryManager repository, IMapper mapper) : IRequestHandler<GetCompanyQuery, CompanyDto>
{
    public async Task<CompanyDto> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
    {
        var company = await repository.Company.GetCompanyAsync(request.Id, request.ChangeTracker);
        if (company is null)
            throw new CompanyNotFoundException(request.Id);

        var companyDto = mapper.Map<CompanyDto>(company);
        return companyDto;
    }
}

internal sealed class CreateCompanyHandler(IRepositoryManager repository, IMapper mapper) : IRequestHandler<CreateCompanyCommand, CompanyDto>
{
    public async Task<CompanyDto> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = mapper.Map<Company>(request.Company);
        repository.Company.CreateCompany(companyEntity);
        await repository.SaveAsync();
        var companyToReturn = mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }
}
internal sealed class UpdateCompanyHandler(IRepositoryManager repository, IMapper mapper) : IRequestHandler<UpdateCompanyCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = await repository.Company.GetCompanyAsync(request.Id, request.ChangeTracker);
        if (companyEntity is null)
            throw new CompanyNotFoundException(request.Id);
        mapper.Map(request.Company, companyEntity);
        await repository.SaveAsync();

        return Unit.Value;
    }
}
internal sealed class DeleteCompanyHandler(IRepositoryManager repository) : IRequestHandler<DeleteCompanyCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyEntity = await repository.Company.GetCompanyAsync(request.Id, request.ChangeTracker);
        if (companyEntity is null)
            throw new CompanyNotFoundException(request.Id);

        repository.Company.DeleteCompany(companyEntity);
        await repository.SaveAsync();

        return Unit.Value;
    }
}