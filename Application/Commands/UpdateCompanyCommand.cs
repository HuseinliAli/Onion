using MediatR;
using Shared.DTOs;

namespace Application.Commands
{
    public sealed record UpdateCompanyCommand(Guid Id, CompanyForUpdateDto Company, bool ChangeTracker):IRequest<Unit>;
}
