using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CompanyEmloyees.Presentation.Controllers
{
    [Route("api")]
    public class RootController(LinkGenerator linkGenerator) : BaseApiController 
    {
        [HttpGet(Name ="GetRoot")]
        public IActionResult GetRoot([FromHeader(Name ="Accept")] string mediaType)
        {
            if (mediaType.Contains("application/vnd.codemaze.apiroot"))
            {
                var list = new List<Link>
                {
                    new Link
                    {
                        Href = linkGenerator.GetUriByName(HttpContext,nameof(GetRoot),new{}),
                        Rel="self",
                        Method="GET"
                    },
                    new Link
                    {
                        Href = linkGenerator.GetUriByName(HttpContext,"GetCompanies",new{}),
                        Rel="copmanies",
                        Method="GET"
                    },
                    new Link
                    {
                        Href = linkGenerator.GetUriByName(HttpContext,"CreateCompany",new{}),
                        Rel="create_company",
                        Method="POST"
                    },
                };
                return Ok(list);
            }
            return NoContent();
        }
    }
    
}
