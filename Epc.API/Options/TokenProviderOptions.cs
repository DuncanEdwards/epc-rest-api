using Microsoft.IdentityModel.Tokens;
using System;

namespace Epc.API.Security
{
    public class TokenProviderOptions
    {
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1);

        public string SecretKey { get; set; }

        public string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256;
    }
}
