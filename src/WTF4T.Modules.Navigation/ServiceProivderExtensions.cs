using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Reflection;
using WTF4T.Modules.Navigation.Binders;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWTF4TNavigation(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(
                     typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly, "WTF4T.Modules.Navigation"));
            });

            var builder = services.BuildServiceProvider();
            var binder = builder.GetServices<IModelBinderProvider>().FirstOrDefault(a => a.GetType() == typeof(SitemapItemModelBinder));

            services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, binder);
            });

            return services;
        }
    }
}