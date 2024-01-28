using Contracts.Hateoas;
using Contracts.Shapers;
using Domain.Models;
using Entities.LinkModels;
using Microsoft.AspNetCore.Routing;
using Shared.DTOs;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
namespace API.Utilities
{
    public class EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper) : IEmployeeLinks
    {
        public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(employeesDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedEmployees(employeesDto, fields, companyId, httpContext, shapedEmployees);
            return ReturnShapedEmployees(shapedEmployees);
        }
        private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields)
            => dataShaper.ShapeData(employeesDto, fields).Select(e => e.Entity).ToList();

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees)
            => new LinkResponse { ShapedEntities=shapedEmployees };

        private LinkResponse ReturnLinkedEmployees(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
        {
            var employeeDtoList = employeesDto.ToList();

            for (int i = 0; i < employeeDtoList.Count(); i++)
            {
                var employeeLinks = CreateLinksForEmployee(httpContext, companyId, employeeDtoList[i].Id, fields);

                shapedEmployees[i].Add("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
            var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);
            return new LinkResponse { HasLinks=true, LinkedEntities=linkedEmployees };

        }

        private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
        {
            employeesWrapper.Links.Add(new Link(linkGenerator.GetUriByAction(httpContext,
           "GetEmployeesForCompany", values: new { }),
            "self",
            "GET"));
            return employeesWrapper;
        }
        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
        {
            var links = new List<Link>
                {
                     new Link(linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany",
                    values: new { companyId, id, fields }),
                                        "self",
                                        "GET"),
                     new Link(linkGenerator.GetUriByAction(httpContext,
                    "DeleteEmployeeForCompany", values: new { companyId, id }),
                     "delete_employee",
                     "DELETE"),
                     new Link(linkGenerator.GetUriByAction(httpContext,
                    "UpdateEmployeeForCompany", values: new { companyId, id }),
                     "update_employee",
                     "PUT"),
                     new Link(linkGenerator.GetUriByAction(httpContext,
                    "PartiallyUpdateEmployeeForCompany", values: new { companyId, id }),
                     "partially_update_employee",
                     "PATCH")
                     };
            return links;
        }
    }
}