﻿using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Controllers
{
    public class CompaniesController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet]
        public IActionResult GetCompanies()
        {
            throw new Exception("Exception");
            var companies = serviceManager.CompanyService.GetAllCompanies(false);
            return Ok(companies);
        }
    }
}
