using System.Collections.Generic;

namespace WTF4T.Modules.Navigation.Models
{
    public interface ISitemapItem
    {
        string Id { get; set; }
        List<SitemapItem> Items { get; set; }
        string PublishedDate { get; set; }
        string Title { get; set; }
        string Type { get; set; }
        string Url { get; set; }
        bool Visible { get; set; }
        Dictionary<string, string> Meta { get; set; }

        void RemoveSiteMap(int maxLevels);

        ISitemapItem FindByUrl(string requestUrl);

        void PrepareBreadcrumb(string requestUrl, string parentUrl = null);
    }
}