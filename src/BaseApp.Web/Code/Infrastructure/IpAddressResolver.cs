using System;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace BaseApp.Web.Code.Infrastructure
{
    public class IpAddressResolver
    {
        private readonly HttpContext HttpContext;

        public IpAddressResolver(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            HttpContext = httpContext;
        }

        public string GetUserHostIp()
        {
            return HttpContext != null ? GetMvcClientIp() : null;
        }

        private string GetMvcClientIp()
        {
            string userHostIp = "";
            try
            {
                userHostIp = HttpContext.Request.Headers["X-Forwarded-For"];
                if (!string.IsNullOrEmpty(userHostIp))
                    return userHostIp;
                
                var remoteAddr = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
                //This is needed to avoid IPv6 IPs, as they may be returned by browser (for example in Windows 7 with IIS 7)
                if (remoteAddr?.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (!string.IsNullOrWhiteSpace(remoteAddr.ToString()))
                        return remoteAddr.ToString();
                }
                    
                //Looks like we are on localhost
                foreach (var ipa in System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ipa.ToString();
                    }
                }
            }
            catch { }

            return userHostIp;
        }
    }

}