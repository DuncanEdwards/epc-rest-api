using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Security
{
    public class TokenProviderOptions
    {

        //TODO:Make configurable
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(30);

        public SigningCredentials SigningCredentials { get; set; }

    }
}
