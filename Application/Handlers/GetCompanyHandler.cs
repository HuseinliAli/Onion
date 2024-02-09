using Application.Queries;
using AutoMapper;
using Contracts.Managers;
using Entities.Exceptions;
using MediatR;
using Shared.DTOs;

namespace Application.Handlers;

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
