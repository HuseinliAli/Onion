using Application.Commands;
using AutoMapper;
using Contracts.Managers;
using Domain.Models;
using MediatR;
using Shared.DTOs;

namespace Application.Handlers;

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
