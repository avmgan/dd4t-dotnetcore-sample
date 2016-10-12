using DD4T.Core.Contracts.DependencyInjection;
using DD4T.Core.DD4T.Utils.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WTF4T.Moduels.Navigation.Providers;
using WTF4T.Moduels.Navigation.Providers.Content;
using WTF4T.Modules.Navigation.Binders;

namespace WTF4T.Modules.Navigation
{
    public class Bootstrap : IDependencyMapper
    {
        public TypeDescriptionList TypeDescriptions()
        {
            var list = new TypeDescriptionList();

            list.Add(typeof(IModelBinderProvider), typeof(SitemapItemModelBinder));

            list.Add(typeof(INavigationProvider), typeof(NavigationProvider));
            list.Add(typeof(IContentProvider), typeof(NavigationContentProvider));

            return list;
        }
    }
}