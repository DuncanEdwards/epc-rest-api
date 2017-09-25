using Epc.API.Helpers;
using Epc.API.Models;
using Epc.API.Options;
using Epc.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net;

namespace Epc.API.Controllers
{
    [Route("api/v1/account")]
    public class AccountController : Controller
    {

        #region Private Fields

        private readonly IEpcRepository _epcRepository;
        private readonly IEmailSender _emailSender;
        private readonly TokenProviderSettings _tokenProviderOptions;
        

        #endregion

        #region Public Constructor

        public AccountController(
            IEpcRepository epcRepository,
            IEmailSender emailSender,
            IOptions<TokenProviderSettings> tokenProviderOptions)
        {
            _epcRepository = epcRepository;
            _emailSender = emailSender;
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

            //TODO: Split out link creation into private message and inc remember tokn

            var htmlMessage = sendResetLinkDto.IsNewUser ?
                $"You have been added to the EPC administration dashboard.<br> To enter a password please click <a href='{GetResetLink(sendResetLinkDto, user.RememberToken.Value)}'>here</a>." :
                $"To reset your password, please click <a href='{GetResetLink(sendResetLinkDto, user.RememberToken.Value)}'>here</a>.";

            var task = _emailSender.SendEmailAsync(
                sendResetLinkDto.Email,
                (sendResetLinkDto.IsNewUser?"Enter a password to activate your account":"Reset your password"),
                htmlMessage);
            return Ok();
        }

        private string GetResetLink(
            SendResetLinkDto sendResetLinkDto,
            Guid rememberToken)
        {
            return sendResetLinkDto.ResetLink + 
                "?IsReset=" + (!sendResetLinkDto.IsNewUser) + 
                "&RememberToken=" + WebUtility.UrlEncode(rememberToken.ToString());
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
