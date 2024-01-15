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
                .ForMember(c=>c.FullAddress,opt=>opt.MapFrom(i=>string.Join(' ',i.Address,i.Country)));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto , Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<CompanyForUpdateDto , Company>();
        }
    }
}
