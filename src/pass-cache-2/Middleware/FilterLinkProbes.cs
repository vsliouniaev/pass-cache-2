using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace pass_cache_2.Middleware
{
    public class FilterLinkProbes
    {
        private static readonly string[] BlockedAgents = { "skype", "whatsapp" };
        private readonly RequestDelegate _next;

        public FilterLinkProbes(RequestDelegate next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            StringValues userAgents;
            if (context.Request.Headers.TryGetValue("User-Agent", out userAgents))
            {
                if (userAgents.Select(a => a.ToLowerInvariant()).Any(userAgent => BlockedAgents.Any(blockedAgent => blockedAgent.Contains(userAgent))))
                {
                    context.Response.Redirect("/", true);
                    return;
                }
            }

            await _next(context);
        }
    }
}
