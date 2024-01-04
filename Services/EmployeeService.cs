using Contracts.Logging;
using Contracts.Managers;
using Services.Contracts;

namespace Services;

internal sealed class EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager) : IEmployeeService
{
}
