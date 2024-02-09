using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public sealed record CreateCompanyCommand(CompanyForCreationDto Company) : IRequest<CompanyDto>;
}
