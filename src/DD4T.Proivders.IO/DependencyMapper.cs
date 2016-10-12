using DD4T.ContentModel.Contracts.Providers;
using DD4T.Core.Contracts.DependencyInjection;
using DD4T.Core.DD4T.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DD4T.Proivders.IO
{
    public class DependencyMapper : IDependencyMapper
    {
        public TypeDescriptionList TypeDescriptions()
        {
            var mappings = new TypeDescriptionList();

            mappings.Add(typeof(IPageProvider), typeof(PageProvider));
            mappings.Add(typeof(ILinkProvider), typeof(LinkProvider));
            mappings.Add(typeof(IComponentPresentationProvider), typeof(ComponentPresentationProvider));
            mappings.Add(typeof(IBinaryProvider), typeof(BinaryProvider));

            return mappings;
        }
    }
}