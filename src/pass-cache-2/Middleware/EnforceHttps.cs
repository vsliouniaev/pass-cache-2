using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

namespace pass_cache_2.Middleware
{
    public class EnforceHttps
    {
        private readonly RequestDelegate _next;

        public EnforceHttps(RequestDelegate next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.IsHttps)
            {
                context.Response.Redirect($"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}", false);
            }

            return _next(context);
        }
    }
}
