using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Epc.API.Middleware
{
    public class AddCustomResponseHeaders
    {
        private readonly RequestDelegate _next;

        public AddCustomResponseHeaders(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            IHeaderDictionary headers = context.Response.Headers;

            headers["Access-Control-Expose-Headers"] = "X-Pagination";

            await _next(context);

        }
    }
}
