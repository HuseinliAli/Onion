using Application.Commands;
using AutoMapper;
using Contracts.Managers;
using Entities.Exceptions;
using MediatR;

namespace Application.Handlers;

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
