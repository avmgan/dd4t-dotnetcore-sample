using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTF4T.Modules.Navigation.Models
{
    public class SitemapItem : ISitemapItem
    {
        public SitemapItem()
        {
            Init();
        }

        public SitemapItem(String title)
        {
            Init();
            Title = title;
        }

        private void Init()
        {
            Items = new List<SitemapItem>();
            Meta = new Dictionary<string, string>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public List<SitemapItem> Items { get; set; }
        public string PublishedDate { get; set; }
        public bool Visible { get; set; }
        public Dictionary<string, string> Meta { get; set; }

        public ISitemapItem FindByUrl(string requestUrl)
        {
            if (this.Url.Equals(requestUrl, StringComparison.CurrentCultureIgnoreCase))
                return this;

            if (!requestUrl.StartsWith(this.Url, StringComparison.CurrentCultureIgnoreCase))
                return null;

            return this.Items.Select(a => a.FindByUrl(requestUrl))
                                .Where(a => !a.IsNull())
                                .FirstOrDefault();
        }

        public void CleanAllUrls(string defaultFileName)
        {
            this.Url = this.Url.CleanUrl(defaultFileName);

            this.Items.ForEach(i => i.CleanAllUrls(defaultFileName));
        }

        public void PrepareBreadcrumb(string requestUrl, string parentUrl = null)
        {
            var item = Items.FirstOrDefault(a => requestUrl.StartsWith(a.Url) && a.Url != parentUrl);

            if (item == null && Items.Any())
            {
                Items.RemoveAll(a => true);
                return;
            }
            if (item == null)
            {
                return;
            }
            Items.RemoveAll(a => a != item);
            item.PrepareBreadcrumb(requestUrl, item.Url);
        }

        public void RemoveSiteMap(int maxLevels)
        {
            this.Items.RemoveAll(i => !i.Visible || (i.Type.Equals("page", StringComparison.CurrentCultureIgnoreCase) && i.PublishedDate == null));
            if (maxLevels == 1)
            {
                this.Items.ForEach(i => i.Items.Clear());
                return;
            }
            maxLevels--;
            this.Items.ForEach(i => i.RemoveSiteMap(maxLevels));
        }
    }
}