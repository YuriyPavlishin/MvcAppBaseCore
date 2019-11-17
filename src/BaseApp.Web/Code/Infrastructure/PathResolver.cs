using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BaseApp.Web.Code.Infrastructure
{
    public interface IPathResolver
    {
        string MapPath(string path);
        string BuildFullUrl(string relativeOrAbsoluteUrl);
    }

    public class PathResolver: IPathResolver
    {
        private readonly IWebHostEnvironment _HostEnv;
        private readonly SiteOptions _siteOptions;

        public PathResolver(IWebHostEnvironment hostEnv, IOptions<SiteOptions> siteOptions)
        {
            _HostEnv = hostEnv;
            _siteOptions = siteOptions.Value;
        }

        public string MapPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return _HostEnv.ContentRootPath;

            if (path.StartsWith("\\"))
                path = path.Substring(1);

            var physicPath = Path.Combine(_HostEnv.ContentRootPath, path);

            return physicPath;
        }

        public string BuildFullUrl(string relativeOrAbsoluteUrl)
        {
            if (relativeOrAbsoluteUrl.StartsWith("~"))
            {
                //TODO: handling relative url
                //relativeOrAbsoluteUrl = VirtualPathUtility.ToAbsolute(relativeOrAbsoluteUrl);
                relativeOrAbsoluteUrl = relativeOrAbsoluteUrl.TrimStart("~".ToCharArray());
            }

            var uri = new Uri(relativeOrAbsoluteUrl, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
            {
                return relativeOrAbsoluteUrl;
            }
            Uri combinedUri;
            if (Uri.TryCreate(BaseUrl(), relativeOrAbsoluteUrl, out combinedUri))
            {
                return combinedUri.AbsoluteUri;
            }
            throw new Exception($"Could not create absolute url for {relativeOrAbsoluteUrl} using baseUri - {BaseUrl()}");
        }

        private Uri BaseUrl()
        {
            var baseUrl = _siteOptions.SiteUrl;
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("SiteUrl not specified in settings");
            return new Uri(baseUrl);
        }
    }
}
