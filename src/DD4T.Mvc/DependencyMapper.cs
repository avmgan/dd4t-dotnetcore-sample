using DD4T.Core.Contracts.DependencyInjection;
using DD4T.Core.DD4T.Utils.Models;
using DD4T.Mvc.Binders;
using DD4T.Mvc.Providers.Content;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace DD4T.Mvc
{
    public class DependencyMapper : IDependencyMapper
    {
        public TypeDescriptionList TypeDescriptions()
        {
            var mappings = new TypeDescriptionList();

            mappings.Add(typeof(IContentProvider), typeof(DefaultContentProvider));
            mappings.Add(typeof(IModelBinderProvider), typeof(DD4TViewModelModelBinder));

            return mappings;
        }
    }
}