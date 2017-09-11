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

namespace Epc.API.Controllers
{
    [Route("api/v1/account")]
    public class AccountController : Controller
    {

        #region Private Fields

        private readonly IEpcRepository _epcRepository;
        private readonly TokenProviderOptions _tokenProviderOptions;

        #endregion

        #region Public Constructor

        public AccountController(
            IEpcRepository epcRepository,
            IOptions<TokenProviderOptions> tokenProviderOptions)
        {
            _epcRepository = epcRepository;
            _tokenProviderOptions = tokenProviderOptions.Value;
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
            var user = _epcRepository.GetUserByEmailAddress(credentials.Username);

            if (user.Password == PasswordHelper.HashPassword(credentials.Password))
            {
                return Ok(TokenHelper.GenerateToken(user, _tokenProviderOptions));
            }

            return BadRequest();
        }

    }
}
