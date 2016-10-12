using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WTF4T.Moduels.Navigation.Providers;
using WTF4T.Modules.Navigation.Models;

namespace WTF4T.Modules.Navigation.Binders
{
    public class SitemapItemModelBinder : IModelBinderProvider, IModelBinder
    {
        private readonly INavigationProvider _navigationProvider;
        private const string MODEL_KEY_FORMAT = "MODEL({0}-{1}-{2}-{3})";

        public SitemapItemModelBinder(INavigationProvider navigationProvider)
        {
            navigationProvider.ThrowIfNull(nameof(navigationProvider));

            _navigationProvider = navigationProvider;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.ThrowIfNull(nameof(bindingContext));

            ISitemapItem model = null;
            //navType & navLevels are set on the 'RouteData' Metadata field on the component template.
            //and passed as routeparameters to the action.
            string navType = null;
            int navLevels = 0;
            int navStartLevel = -1;

            ValueProviderResult value = bindingContext.ValueProvider.GetValue("navType");
            ValueProviderResult navLevelsResult = bindingContext.ValueProvider.GetValue("navLevels");
            ValueProviderResult navStartLevelResult = bindingContext.ValueProvider.GetValue("navStartLevel");

            if (value.Any())
            {
                navType = value.FirstValue;
            }

            string requestUrlPath = bindingContext.HttpContext.Request.Path.Value;

            ////Filename should be removed. the data published from Tridion doesnt include the filename into the url's
            ////i.e. for url '/product/index.html' published content from Tridion contains: '/products/'
            //var fileName = Path.GetFileName(requestUrlPath);
            //if (!fileName.IsNullOrEmpty())
            //    requestUrlPath = requestUrlPath.Replace(fileName, string.Empty);

            if (navLevelsResult.Any())
                int.TryParse(navLevelsResult.FirstValue, out navLevels);
            if (navStartLevelResult.Any())
                int.TryParse(navStartLevelResult.FirstValue, out navStartLevel);

            //generate a Cache key for the current model.
            //try to get model out of HttpContext.Items[], incase the same model is request multiple times during
            //a single request.
            string key = MODEL_KEY_FORMAT.FormatString(navType, requestUrlPath, navLevels, navStartLevel);

            model = bindingContext.HttpContext.Items[key] as ISitemapItem;

            if (model == null)
            {
                switch (navType?.ToLower())
                {
                    case "children":
                        model = _navigationProvider.GetChildren(requestUrlPath, navLevels, navStartLevel);
                        break;

                    case "path":
                        model = _navigationProvider.GetPath(requestUrlPath);
                        break;

                    default:
                        model = _navigationProvider.GetAll(navLevels);
                        break;
                }
                bindingContext.HttpContext.Items[key] = model;
            }
            //Todo: WTF... this doesn't make sense; Bug in Microsoft.AspNetCore.Mvc???
            bindingContext.Result = ModelBindingResult.Success(model);
            //Todo: implemenet async
            return Task.FromResult(ModelBindingResult.Success(model));
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            context.ThrowIfNull(nameof(context));

            if (typeof(ISitemapItem).IsAssignableFrom(context.Metadata.ModelType))
                return this;

            return null;
        }
    }
}