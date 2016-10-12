using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DD4T.SampleWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("dd4t-config.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();

            var d = Configuration.GetSection("DD4TConfiguration");
        }

        private IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visith http://go.microsoft.com/fwlink/?LinkID=398940ttp://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMvc();
            services.AddDD4T();

            services.AddWTF4TNavigation();
            services.AddDD4TModelBinders();

            //services.Configure<RazorViewEngineOptions>(options =>
            //{
            //    options.FileProviders.Add(new EmbeddedFileProvider(
            //         typeof(NavigationProvider).GetTypeInfo().Assembly, "WTF4T.Modules.Navigation"));
            //});

            //services.AddOptions();
            services.Configure<MyDD4TConfiguration>(options => Configuration.GetSection("DD4TConfiguration").Bind(options));
            //services.AddDD4T();

            //var builder = new ContainerBuilder();

            ////builder.RegisterType<MyDD4TConfiguration>()
            ////    .As<IDD4TConfiguration>()
            ////    .SingleInstance();

            //builder.AddDD4T();
            //builder.Populate(services);
            //var container = builder.Build();

            //return new AutofacServiceProvider(container);

            //services.AddSingleton<IPublicationResolver, test>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseDD4T();

            //appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}