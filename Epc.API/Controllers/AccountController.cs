using Epc.API.Helpers;
using Epc.API.Models;
using Epc.API.Security;
using Epc.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

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


        [HttpPut("changepassword/{userId}")]
        public IActionResult ChangePassword(
            Guid userId, 
            [FromBody] ChangePasswordDto changePasswordDto)
        {

            var user = _epcRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Password == PasswordHelper.HashPassword(changePasswordDto.OldPassword))
            {
                user.Password = PasswordHelper.HashPassword(changePasswordDto.NewPassword);
                _epcRepository.Save();
                return NoContent();
            }
            return Forbid();
        }

        [HttpPost("Token")]
        public IActionResult Token([FromBody] CredentialsDto credentials)
        {
            var user = _epcRepository.GetUserByEmailAddress(credentials.Username);

            if ((user != null) && (user.Password == PasswordHelper.HashPassword(credentials.Password)))
            {
                return Ok(TokenHelper.GenerateToken(user, _tokenProviderOptions));
            }

            return Forbid();
        }

    }
}
