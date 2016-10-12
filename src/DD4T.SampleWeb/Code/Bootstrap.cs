using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Core.Contracts.DependencyInjection;
using DD4T.Core.Contracts.ViewModels;
using DD4T.Core.DD4T.Utils.Models;
using DD4T.Mvc.Binders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Bootstrap
    {
        public static IServiceCollection AddDD4T(this IServiceCollection services)
        {
            var loadedAssemblies = GetReferencingAssemblies();

            var mappers = loadedAssemblies
                            .SelectMany(s => s.GetTypes())
                            .Where(p => typeof(IDependencyMapper).IsAssignableFrom(p) && !p.GetTypeInfo().IsInterface)
                            .Select(o => Activator.CreateInstance(o) as IDependencyMapper).Distinct();

            foreach (var mapper in mappers)
            {
                var types = mapper.TypeDescriptions();
                services.RegisterTypes(types);
            }

            return services;
        }

        public static IServiceCollection AddDD4TModelBinders(this IServiceCollection services)
        {
            var builder = services.BuildServiceProvider();
            var typedBinder = builder.GetServices<IModelBinderProvider>().FirstOrDefault(binder => binder.GetType() == typeof(DD4TViewModelModelBinder));

            var viewmodel = builder.GetService<IViewModelFactory>();
            viewmodel.LoadViewModels(GetReferencingAssemblies());

            services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, typedBinder);
            });
            return services;
        }

        private static void RegisterTypes(this IServiceCollection services, TypeDescriptionList types)
        {
            foreach (var item in types)
            {
                switch (item.LifeCycle)
                {
                    case LifeCycle.PerRequest:
                        services.AddTransient(item.Interface, item.Implementation);
                        break;

                    case LifeCycle.SingleInstance:
                        services.AddSingleton(item.Interface, item.Implementation);
                        break;

                    case LifeCycle.PerLifeTime:
                        services.AddTransient(item.Interface, item.Implementation);
                        break;

                    case LifeCycle.PerDependency:
                        services.AddScoped(item.Interface, item.Implementation);
                        break;

                    default:
                        break;
                }
            }
        }

        public static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();

            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies.Where(a => a.Name.StartsWith("DD4T.") || a.Name.StartsWith("WTF4T.")))
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
            return assemblies;
        }
    }
}