using AutoMapper;
using Contracts.Logging;
using Contracts.Managers;
using Domain.Models;
using Services.Contracts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
internal sealed class CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager,IMapper mapper) : ICompanyService
{
    public IEnumerable<CompanyDto> GetAllCompanies(bool changeTracker)
    {
		try
		{
			var companies = repositoryManager.Company.GetAllCompanies(changeTracker);
			var companiesDto = mapper.Map<IEnumerable<CompanyDto>>(companies);
			return companiesDto;
		}
		catch (Exception ex)
		{
			loggerManager.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
			throw;
		}
    }
}
