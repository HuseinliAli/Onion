using Application.Commands;
using Application.Notifications;
using Application.Queries;
using AutoMapper;
using Contracts.Logging;
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
internal sealed class DeleteCompanyHandler(IRepositoryManager repository) :INotificationHandler<CompanyDeleteNotification>
{
    public async Task Handle(CompanyDeleteNotification notification, CancellationToken cancellationToken)
    {
        var companyEntity = await repository.Company.GetCompanyAsync(notification.Id, notification.ChangeTracker);
        if (companyEntity is null)
            throw new CompanyNotFoundException(notification.Id);

        repository.Company.DeleteCompany(companyEntity);
        await repository.SaveAsync(); 
    }
}
internal sealed class EmailHandler(ILoggerManager logger) : INotificationHandler<CompanyDeleteNotification>
{
    public async Task Handle(CompanyDeleteNotification notification, CancellationToken cancellationToken)
    {
        logger.LogWarning($"Delete action for company with id: {notification.Id}");
        await Task.CompletedTask;
    }
}