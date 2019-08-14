using System.IO;
using AutoMapper;
using EdmsMockApi.Caching;
using EdmsMockApi.Data;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Infrastructure;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace EdmsMockApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppContext>(options => options
               .UseLazyLoadingProxies()
               .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddSingleton<MemoryCacheManager>();
            services.AddSingleton<ILocker>(provider => provider.GetRequiredService<MemoryCacheManager>());
            services.AddSingleton<ICacheManager>(provider => provider.GetRequiredService<MemoryCacheManager>());

            services.AddAutoMapper();
            services.AddMediatR();
            services.AddApis();

            services.AddScoped<IDbContext>(provider => new AppContext(provider.GetRequiredService<DbContextOptions<AppContext>>()));
            services.AddScoped(typeof(IRepository<>), typeof(AppRepository<>));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddVersionedApiExplorer(version => version.GroupNameFormat = "'v'VVV");
            services.AddMvc()
                .AddFeatureFolders()
                .AddFluentValidation(config => { config.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            services.AddSwaggerGen(
                options =>
                {
                    options.AddApiVersion(services);
                    options.AddSwaggerGenOptions();

                    var filePath = Path.Combine(System.AppContext.BaseDirectory, "EdmsMockApi.xml");
                    options.IncludeXmlComments(filePath);
                });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ProjectPath = Path.Combine(env.ContentRootPath, @"app"),
                    ConfigFile = Path.Combine(env.ContentRootPath, @"app/build/webpack.dev.conf.js")
                });
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";
                        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (errorFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                        }

                        await context.Response.WriteAsync("There was an error");

                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });

            app.UseCors("AllowAllOrigins");
            app.UseMvc(routes =>
         {
             routes.MapRoute(
                 name: "default",
                 template: "{controller=Home}/{action=Index}/{id?}");

             routes.MapSpaFallbackRoute(
                 name: "spa-fallback",
                 defaults: new { controller = "Home", action = "Index" });
         });
        }
    }
}