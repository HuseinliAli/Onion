using AutoMapper;
using Contracts.Hateoas;
using Contracts.Logging;
using Contracts.Managers;
using Contracts.Shapers;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Contracts;
using Shared.DTOs;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    public ServiceManager(IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper,
        IDataShaper<EmployeeDto> dataShaper,
        IEmployeeLinks employeeLinks,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _companyService = new Lazy<ICompanyService>(()=>
                    new CompanyService(repositoryManager, loggerManager,mapper));
        
        _employeeService = new Lazy<IEmployeeService>(() =>
                    new EmployeeService(repositoryManager, loggerManager,mapper, dataShaper,employeeLinks));

        _authenticationService = new Lazy<IAuthenticationService>(() =>
        
            new AuthenticationService(loggerManager,mapper,userManager,configuration)
        );


    }

    public ICompanyService CompanyService => _companyService.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}