using Microsoft.AspNetCore.Mvc;

namespace Epc.API.Controllers
{
    [Route("api/v1/test")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new { state = "success" });
        }

    }
}