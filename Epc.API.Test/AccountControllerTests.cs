using Epc.API.Services;
using Xunit;
using Moq;
using Epc.API.Options;
using Epc.API.Controllers;
using Epc.API.Models;
using Epc.API.Entities;
using Epc.API.Helpers;
using System;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Epc.API.Test
{
    public class AccountControllerTests
    {
        [Fact]
        public void SuccessfullyGetTokenForValidUser()
        {
            var rawValidPassword = "valid";

            var validUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test_user@yahoo.com",
                FirstName = "Test",
                Surname = "User",
                Password = PasswordHelper.HashPassword(rawValidPassword),
                Type = new UserType() { Code = "ADMINISTRATOR", Name = "Administrator" }
            };

            var mockEmailSender = new Mock<IEmailSender>();

            //Mock provider options
            var tokenProviderSettings = new TokenProviderSettings
            {
                SecretKey = "testsecretneedstobelong"
            };

            var tokenProviderOptions = Microsoft.Extensions.Options.Options.Create<TokenProviderSettings>(tokenProviderSettings);


            var mockEpcRepository = new Mock<IEpcRepository>();
            mockEpcRepository.Setup(r => r.GetUserByEmailAddress(It.Is<string>(e => e == validUser.Email))).Returns(validUser);
            //mockEpcRepository.Setup(r => r.GetUserByEmailAddress().Returns<User>(r => (validUser));

            var accountController = new AccountController(
                mockEpcRepository.Object,
                mockEmailSender.Object,
                tokenProviderOptions);

            var credentialsDto = new CredentialsDto() { Username = validUser.Email, Password = rawValidPassword };

            var result = accountController.Token(credentialsDto);

            Assert.IsType<OkObjectResult>(result);


            //Validate token
            var tokenDto = ((OkObjectResult)result).Value as TokenDto;
            Assert.NotNull(tokenDto);
            Assert.Equal<string>("Bearer", tokenDto.TokenType);

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenDto.AccessToken);

            var idClaim = jwtToken.Claims.First(c => c.Type == "sub");
            Assert.Equal<string>(validUser.Id.ToString(), idClaim.Value);

            var givenNameClaim = jwtToken.Claims.First(c => c.Type == "given_name");
            Assert.Equal<string>(validUser.FirstName.ToString(), givenNameClaim.Value);

            var familyNameClaim = jwtToken.Claims.First(c => c.Type == "family_name");
            Assert.Equal<string>(validUser.Surname.ToString(), familyNameClaim.Value);

            var roleClaim = jwtToken.Claims.First(c => c.Type == "role");
            Assert.Equal<string>("Administrator", roleClaim.Value);
        }

        [Fact]
        public void FailToGetTokenForInvalidUser()
        {
            var rawValidPassword = "valid";

            var validUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test_user@yahoo.com",
                FirstName = "Test",
                Surname = "User",
                Password = PasswordHelper.HashPassword(rawValidPassword),
                Type = new UserType() { Code = "ADMINISTRATOR", Name = "Administrator" }
            };

            var mockEmailSender = new Mock<IEmailSender>();

            //Mock provider options
            var tokenProviderSettings = new TokenProviderSettings
            {
                SecretKey = "testsecretneedstobelong"
            };

            var tokenProviderOptions = Microsoft.Extensions.Options.Options.Create<TokenProviderSettings>(tokenProviderSettings);


            var mockEpcRepository = new Mock<IEpcRepository>();
            mockEpcRepository.Setup(r => r.GetUserByEmailAddress(It.Is<string>(e => e == validUser.Email))).Returns<User>(null);

            var accountController = new AccountController(
                mockEpcRepository.Object,
                mockEmailSender.Object,
                tokenProviderOptions);

            var credentialsDto = new CredentialsDto() { Username = validUser.Email, Password = rawValidPassword };

            var result = accountController.Token(credentialsDto);

            Assert.IsType<ForbidResult>(result);
        }
    }
}