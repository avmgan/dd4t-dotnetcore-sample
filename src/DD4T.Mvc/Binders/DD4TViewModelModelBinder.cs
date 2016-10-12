using DD4T.Core.Contracts.ViewModels;
using DD4T.Mvc.Providers.Content;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DD4T.Mvc.Binders
{
    public class DD4TViewModelModelBinder : IModelBinderProvider, IModelBinder
    {
        private readonly IContentProvider _contentProvider;
        private const string MODEL_KEY_FORMAT = "MODEL({0})";

        public DD4TViewModelModelBinder(IContentProvider contentProvider)
        {
            contentProvider.ThrowIfNull(nameof(contentProvider));

            _contentProvider = contentProvider;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.ThrowIfNull(nameof(bindingContext));

            IViewModel model = null;
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            //value is n
            if (!value.Any())
                return null;

            var passedModel = value;

            if (passedModel != null && bindingContext.ModelType.IsAssignableFrom(passedModel.GetType()))
            {
                //route already available and of the correct type so return it.
                return null;
            }

            string modelData = string.IsNullOrEmpty(passedModel.FirstValue) ? "/" : passedModel.FirstValue;
            //return null if modeldata is not set.
            if (modelData == null) return null;

            //generate a Cache key for the current model.
            //try to get model out of HttpContext.Items[], incase the same model is request multiple times during
            //a single request.
            string key = MODEL_KEY_FORMAT.FormatString(modelData);
            //model = bindingContext.HttpContext.Items[key] as IViewModel;
            if (model == null)
            {
                model = modelData.StartsWith("tcm:") ?
                            _contentProvider.BuildViewModel(new TcmUri(modelData)) :
                            _contentProvider.BuildViewModel(modelData);

                if (model != null && !bindingContext.ModelType.IsAssignableFrom(model.GetType()))
                {
                    //Model is incorrect, so remove
                    model = null;
                }

                bindingContext.HttpContext.Items[key] = model;
            }
            //if (model is IRenderable)
            //    controllerContext.RouteData.DataTokens["area"] = ((IRenderable)model).MvcData.Area;

            //Todo: WTF... this doesn't make sense; Bug in Microsoft.AspNetCore.Mvc???
            bindingContext.Result = ModelBindingResult.Success(model);
            //Todo: implemenet async
            return Task.FromResult(ModelBindingResult.Success(model));
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            context.ThrowIfNull(nameof(context));

            if (typeof(IViewModel).IsAssignableFrom(context.Metadata.ModelType))
                return this;

            return null;
        }
    }
}