using Microsoft.IdentityModel.Tokens;
using System;

namespace Epc.API.Options
{
    public class TokenProviderSettings
    {
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1);

        public string SecretKey { get; set; }

        public string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256;
    }
}
