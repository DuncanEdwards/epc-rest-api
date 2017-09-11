using Epc.API.Entities;
using Epc.API.Security;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Epc.API.Helpers
{
    public class TokenHelper
    {

        #region Private Fields

        private readonly TokenProviderOptions _tokenProviderOptions;

        #endregion

        #region Public Constructor

        public TokenHelper(IOptions<TokenProviderOptions> tokenProviderOptionsAccessor)
        {
            _tokenProviderOptions = tokenProviderOptionsAccessor.Value;
        }

        #endregion

        #region Public Methods

        public object GenerateToken(
            string emailAddress, 
            string firstName, 
            string lastName,
            UserType userType)
        {
            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), sub (subject/user) claims.
            // First and last names allow the UI to display the loggin in user (if they want) and roles are useful for authorization of other controllers
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, emailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, firstName),
                new Claim(JwtRegisteredClaimNames.FamilyName , lastName),
                new Claim(JwtRegisteredClaimNames.Iat, ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role, userType.Name),
                new Claim("role", userType.Name) //This is useful for the react frontend that doesn't want to deal with the namespace
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.Add(_tokenProviderOptions.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                token_type,
                expires_in = (int)_options.Expiration.TotalSeconds
            };
            return tokenResponse
        }


        #endregion


        #region Private Methods

        private void GetSigningCredentials()
        {

        }

        #endregion
    }
}
