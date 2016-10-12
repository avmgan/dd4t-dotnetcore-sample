using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DD4T.Proivders.IO
{
    public class BinaryProvider : IBinaryProvider
    {
        public int PublicationId
        {
            get; set;
        }

        public byte[] GetBinaryByUri(string uri)
        {
            return null;
        }

        public byte[] GetBinaryByUrl(string url)
        {
            return null;
        }

        public IBinaryMeta GetBinaryMetaByUri(string uri)
        {
            return null;
        }

        public IBinaryMeta GetBinaryMetaByUrl(string url)
        {
            return null;
        }

        public Stream GetBinaryStreamByUri(string uri)
        {
            return null;
        }

        public Stream GetBinaryStreamByUrl(string url)
        {
            return null;
        }

        public DateTime GetLastPublishedDateByUri(string uri)
        {
            return DateTime.Now; ;
        }

        public DateTime GetLastPublishedDateByUrl(string url)
        {
            return DateTime.Now;
        }

        public string GetUrlForUri(string uri)
        {
            return null;
        }
    }
}