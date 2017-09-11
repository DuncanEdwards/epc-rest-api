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

namespace Epc.API.Controllers
{
    [Route("api/v1/account")]
    public class AccountController : Controller
    {

        #region Private Fields

        private IEpcRepository _epcRepository;

        #endregion

        #region Public Constructor

        public AccountController(IEpcRepository epcRepository)
        {
            _epcRepository = epcRepository;
        }

        #endregion

        public class AuthorForCreationDto
        {
            public string Email { get; set; }
        }


        [HttpPut("Password")]
        public IActionResult Password([FromBody]AuthorForCreationDto email)
        {
            return new StatusCodeResult(StatusCodes.Status202Accepted);
        }

        [HttpPost("Token")]
        public IActionResult Token([FromBody] CredentialsDto credentials)
        {
            var user = _epcRepository.GetUserByEmailAddress("dun_edwards@yahoo.com");

            if (user.Password == PasswordHelper.HashPassword(credentials.Password))
            {
                return Ok(new { token = "blahblahblah" });
            }

            return BadRequest();
        }

    }
}
