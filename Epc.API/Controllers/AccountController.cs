using Epc.API.Helpers;
using Epc.API.Models;
using Epc.API.Security;
using Epc.API.Services;
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

        #region Public Actions

        [HttpPut("{userId}/ChangePassword")]
        public IActionResult ChangePassword(
            Guid userId, 
            [FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                //return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

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

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                //return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var user = _epcRepository.GetUserByRememberToken(resetPasswordDto.RememberToken);

            if (user == null)
            {
                return Forbid();
            }

            user.Password = PasswordHelper.HashPassword(resetPasswordDto.Password);
            _epcRepository.Save();
            return NoContent();
        }

        [HttpPost("SendResetLink")]
        public IActionResult SendResetLink([FromBody] SendResetLinkDto sendResetLinkDto)
        {
            if (!ModelState.IsValid)
            {
                //return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var user = _epcRepository.GetUserByEmailAddress(sendResetLinkDto.Email);

            if (user == null)
            {
                return Forbid();
            }
            user.RememberToken = Guid.NewGuid();
            _epcRepository.Save();
            return Ok();
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

        #endregion

    }
}
