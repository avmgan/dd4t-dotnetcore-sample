using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using WTF4T.Moduels.Navigation.Providers.Content;
using WTF4T.Modules.Navigation.Models;

namespace WTF4T.Moduels.Navigation.Providers
{
    public class NavigationProvider : INavigationProvider
    {
        private readonly IContentProvider _contentProvider;
        private readonly ILogger _logger;
        private readonly IDD4TConfiguration _configuration;

        private const string URL = "navigation"; //TODO: should this be configurabale?

        public NavigationProvider(IContentProvider contentProvider, ILogger logger, IDD4TConfiguration configuration)
        {
            contentProvider.ThrowIfNull(nameof(contentProvider));
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));

            _configuration = configuration;
            _contentProvider = contentProvider;
            _logger = logger;
        }

        public virtual ISitemapItem GetChildren(string requestUrlPath, int levels = 0, int startLevel = -1)
        {
            var result = Load();

            if (startLevel != -1)
            {
                var urlWithoutPublication = requestUrlPath.Remove(0, result.Url.Length);
                var segments = urlWithoutPublication.Split('/').Where(s => !s.IsNullOrEmpty());
                if (segments.Count() < startLevel)
                    return null;

                segments = segments.Take(startLevel).ToArray();

                requestUrlPath = string.Concat(string.Concat(result.Url, string.Join("/", segments)).TrimEnd('/'), "/");
            }
            if (levels == 0) levels = 10;

            result = result.FindByUrl(requestUrlPath);
            result?.RemoveSiteMap(levels);

            return result;
        }

        public virtual ISitemapItem GetAll(int levels = 0)
        {
            var result = Load();

            if (levels == 0) levels = 10;

            result.RemoveSiteMap(levels);

            return result;
        }

        public virtual ISitemapItem GetPath(string requestUrlPath)
        {
            var result = Load();
            result.PrepareBreadcrumb(requestUrlPath);
            return result;
        }

        protected virtual ISitemapItem Load()
        {
            var navigationJsonString = _contentProvider.GetPageContent(URL);
            if (navigationJsonString.IsNullOrEmpty())
            {
                _logger.Error("Navigation not found. is the navigation page published? url: {0}".FormatString(URL));
                return null;
            }
            var result = JsonConvert.DeserializeObject<SitemapItem>(navigationJsonString);

            return result;
        }
    }
}