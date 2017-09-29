using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Epc.API.Controllers
{
    [Route("api/v1/test")]
    [Authorize]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new { state = "success" });
        }

    }
}