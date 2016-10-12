using DD4T.ContentModel.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DD4T.Proivders.IO
{
    public class LinkProvider : ILinkProvider
    {
        public int PublicationId
        {
            get; set;
        }

        public string ResolveLink(string componentUri)
        {
            return "/resolved/link/";
        }

        public string ResolveLink(string sourcePageUri, string componentUri, string excludeComponentTemplateUri)
        {
            return "/resolved/link/";
        }
    }
}