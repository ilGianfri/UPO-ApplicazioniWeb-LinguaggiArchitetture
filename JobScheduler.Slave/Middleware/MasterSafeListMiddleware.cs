using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JobScheduler.Slave.Middleware
{
    public class MasterSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MasterSafeListMiddleware> _logger;
        private readonly string _safelist;

        public MasterSafeListMiddleware(RequestDelegate next, ILogger<MasterSafeListMiddleware> logger, string safelist)
        {
            _safelist = safelist;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != HttpMethod.Get.Method)
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

                string[] ip = _safelist.Split(';');

                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach (var address in ip)
                {
                    var testIp = IPAddress.Parse(address);
                    if (testIp.GetAddressBytes().SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning("Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}