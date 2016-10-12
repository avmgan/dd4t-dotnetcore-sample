using DD4T.ContentModel.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DD4T.Proivders.IO
{
    public class PageProvider : IPageProvider
    {
        private const string Path = @"D:\dev\dd4t\vnext\DD4T.SampleWeb\src\DD4T.Proivders.IO\data\{0}";
        public int PublicationId { get; set; }

        public string[] GetAllPublishedPageUrls(string[] includeExtensions, string[] pathStarts)
        {
            throw new NotImplementedException();
        }

        public string GetContentByUri(string uri)
        {
            throw new NotImplementedException();
        }

        public string GetContentByUrl(string url)
        {
            return File.ReadAllText(string.Format(Path, url));
        }

        public DateTime GetLastPublishedDateByUri(string uri)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastPublishedDateByUrl(string url)
        {
            throw new NotImplementedException();
        }
    }
}