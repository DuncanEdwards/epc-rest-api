using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAddCustomResponseHeaders(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AddCustomResponseHeaders>();
        }
    }
}
