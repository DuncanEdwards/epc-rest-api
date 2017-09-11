using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Epc.API.Models;
using Epc.API.Services;
using System.Text;
using System.Security.Cryptography;
using Epc.API.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Epc.API.Security;
using Microsoft.AspNetCore.Authorization;

namespace Epc.API.Controllers
{
    [Route("api/v1/test")]
    [Authorize]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }

    }
}
