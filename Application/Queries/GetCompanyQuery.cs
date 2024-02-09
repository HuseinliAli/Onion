using MediatR;
using Shared.DTOs;

namespace Application.Queries
{
    public sealed record GetCompanyQuery(Guid Id, bool ChangeTracker):IRequest<CompanyDto>;
 }
