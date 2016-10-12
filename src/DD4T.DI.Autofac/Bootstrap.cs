using Autofac;
using DD4T.Core.Contracts.DependencyInjection;
using DD4T.Core.DD4T.Utils.Models;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DD4T.DI.Autofac
{
    public static class Bootstrap
    {
        public static void AddDD4T(this ContainerBuilder builder)
        {
            var loadedAssemblies = GetReferencingAssemblies();

            var mappers = loadedAssemblies
                            .SelectMany(s => s.GetTypes())
                            .Where(p => typeof(IDependencyMapper).IsAssignableFrom(p) && !p.GetTypeInfo().IsInterface)
                            .Select(o => Activator.CreateInstance(o) as IDependencyMapper).Distinct();

            foreach (var mapper in mappers)
            {
                var types = mapper.TypeDescriptions();
                builder.RegisterTypes(types);
            }
        }

        private static void RegisterTypes(this ContainerBuilder builder, TypeDescriptionList types)
        {
            foreach (var item in types)
            {
                switch (item.LifeCycle)
                {
                    case LifeCycle.PerRequest:
                        builder.RegisterType(item.Implementation)
                           .As(item.Interface)
                           .InstancePerRequest()
                           .PreserveExistingDefaults();
                        break;

                    case LifeCycle.SingleInstance:
                        builder.RegisterType(item.Implementation)
                            .As(item.Interface)
                            .SingleInstance()
                            .PreserveExistingDefaults();
                        break;

                    case LifeCycle.PerLifeTime:
                        builder.RegisterType(item.Implementation)
                           .As(item.Interface)
                           .InstancePerLifetimeScope()
                           .PreserveExistingDefaults();
                        break;

                    case LifeCycle.PerDependency:
                        builder.RegisterType(item.Implementation)
                          .As(item.Interface)
                          .InstancePerDependency()
                          .PreserveExistingDefaults();
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

            foreach (var library in dependencies.Where(a => a.Name.StartsWith("DD4T.")))
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
            return assemblies;
        }
    }
}