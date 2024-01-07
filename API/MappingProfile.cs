using AutoMapper;
using Domain.Models;
using Shared.DTOs;

namespace API
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForCtorParam("FullAddress",opt=>opt.MapFrom(i=>string.Join(' ',i.Address,i.Country)));
            CreateMap<Employee, EmployeeDto>();
        }
    }
}
