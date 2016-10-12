using DD4T.ContentModel.Contracts.Providers;
using DD4T.ContentModel.Querying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DD4T.Proivders.IO
{
    public class ComponentPresentationProvider : IComponentPresentationProvider
    {
        public int PublicationId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> FindComponents(IQuery queryParameters)
        {
            throw new NotImplementedException();
        }

        public string GetContent(string uri, string templateUri = "")
        {
            throw new NotImplementedException();
        }

        public List<string> GetContentMultiple(string[] componentUris)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastPublishedDate(string uri)
        {
            throw new NotImplementedException();
        }
    }
}