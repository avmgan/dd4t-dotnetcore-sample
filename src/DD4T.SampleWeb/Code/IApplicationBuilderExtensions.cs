using DD4T.Core.Contracts.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDD4T(this IApplicationBuilder app, Action<IRouteBuilder> configureRoutes = null)
        {
            var dd4tRoute = new RouteBuilder(app, app.ApplicationServices.GetRequiredService<MvcRouteHandler>());

            //If additional routes are configured, add them as well
            if (configureRoutes != null)
                configureRoutes.Invoke(dd4tRoute);

            dd4tRoute.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { controller = "AwesomeController", action = "DoIt" });

            //Always map the default route and apply the favicon ignore constaint
            dd4tRoute.MapRoute(
               "DefaultPage",
               "{*page}",
               new { controller = "Page", action = "Index" }
            );

            //dd4tRoute.MapRoute(
            //        name: "TridionPage",
            //        //template: "{*PageUrl:faviconIgnoreConstraint}",
            //        template: "{*page}",
            //        defaults: new { controller = "Page", action = "Index" } // action = "PageAsync" }
            //        //defaults: new { controller = "ComponentPresentation", action = "CP" }
            //        );
            var routes = dd4tRoute.Build();

            app.UseMiddleware<IgnoreFaviconMiddleware>();
            app.UseMiddleware<DD4TMiddleWare>(routes);

            return app;
        }
    }

    public class IgnoreFaviconMiddleware
    {
        private readonly RequestDelegate next;

        // You can inject a dependency here that gives you access
        // to your ignored route configuration.
        public IgnoreFaviconMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.Contains("favicon.ico"))
            {
                context.Response.StatusCode = 404;
                return;
            }

            await next.Invoke(context);
        }
    }

    public class DD4TMiddleWare
    {
        private readonly IRouter _router;
        private readonly RequestDelegate _next;

        public DD4TMiddleWare(RequestDelegate next, IRouter router)
        {
            _next = next;
            _router = router;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var context = new RouteContext(httpContext);
            context.RouteData.Routers.Add(_router);

            await _router.RouteAsync(context);

            if (context.Handler == null)
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                httpContext.Features[typeof(IRoutingFeature)] = new RoutingFeature()
                {
                    RouteData = context.RouteData,
                };
                await context.Handler(context.HttpContext);
            }
        }
    }

    public class RoutingFeature : IRoutingFeature
    {
        public RouteData RouteData { get; set; }
    }
}