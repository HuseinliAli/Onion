using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public sealed record GetCompaniesQuery(bool ChangeTracker):IRequest<IEnumerable<CompanyDto>>;
 }
