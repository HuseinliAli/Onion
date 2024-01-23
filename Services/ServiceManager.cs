using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Contracts.Shapers;
using Services.Contracts;
using Shared.DTOs;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager loggerManager,IMapper mapper,IDataShaper<EmployeeDto> dataShaper)
    {
        _companyService = new Lazy<ICompanyService>(()=>
                    new CompanyService(repositoryManager, loggerManager,mapper));
        
        _employeeService = new Lazy<IEmployeeService>(() =>
                    new EmployeeService(repositoryManager, loggerManager,mapper, dataShaper));
    }

    public ICompanyService CompanyService => _companyService.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;
}