using Contracts.Logging;
using Contracts.Managers;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
internal sealed class CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager) : ICompanyService
{
}
