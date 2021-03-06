﻿using Epc.API.Entities;
using Epc.API.Models;
using Epc.API.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Epc.API.Helpers
{
    static public class TokenHelper
    {

        #region Public Methods

        static public TokenDto GenerateToken(
            User user,
            TokenProviderSettings tokenProviderOptions)
        {
            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), sub (subject/user) claims.
            // First and last names allow the UI to display the loggin in user (if they want) and roles are useful for authorization of other controllers
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName , user.Surname),
                new Claim(JwtRegisteredClaimNames.Iat, ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role, user.Type.Name),
                new Claim("role", user.Type.Name) //This is useful for the react frontend that doesn't want to deal with the namespace
            };

            var tokenExpiration = now.Add(tokenProviderOptions.Expiration);

            // Create the JWT and write it to a string
            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: tokenExpiration,
                signingCredentials: GetSigningCredentials(tokenProviderOptions));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new TokenDto()
            {
                AccessToken = accessToken,
                ExpiresIn = ((Int32)tokenExpiration.Subtract(new DateTime(1970, 1, 1)).TotalSeconds),
            };
        }

        static public TokenValidationParameters GetTokenValidationParameters(TokenProviderSettings tokenProviderOptions)
        {
            return new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSymmetricSecurityKey(tokenProviderOptions.SecretKey),

                // Validate the token expiry
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
        }

        #endregion


        #region Private Methods

        static private SigningCredentials GetSigningCredentials(TokenProviderSettings tokenProviderOptions)
        {
            return new SigningCredentials(
                GetSymmetricSecurityKey(tokenProviderOptions.SecretKey),
                tokenProviderOptions.Algorithm);
        }

        static private SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

        #endregion
    }
}
