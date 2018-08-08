using GlobalConfiguration;
using Interfaces.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace AspNetCore_SPA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // MVC and XSRF
            services.AddMvc(
            //options =>
            //{
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); // XSRF seems to be bugged right now with this combination of ASP.NET Core / Angular 6 versions. To be reviewed later on
            //}
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddHsts(options =>
            {
                options.Preload = false ;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(1);
            }); // on Prod, this should be preloaded and MaxAge set to at least 2 years

            // Data access
            services.AddDbContext<SpaContext>(options => options.UseInMemoryDatabase("InMemoryDb"), ServiceLifetime.Scoped); // using in-memory db gives us opportunity to write abstraction early on and then easily change data store

            // Dependency Injection
            services.AddSingleton<IGlobalConfig, GlobalConfig>();
            services.AddScoped<ITaskRepository, TaskRepository>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "AspNetCore_SPA-API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCore_SPA-API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Add security rules
            app.UseHttpsRedirection();
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());
            app.UseCsp(opts => opts
                .BlockAllMixedContent()
                .DefaultSources(s => s.Self())
                .ScriptSources(x => x.Self().UnsafeEval()) // unsafe required for Angular
                .StyleSources(x => x.Self().UnsafeInline()) // unsafe required for Angular
                .ConnectSources(x => x.Self().CustomSources("wss://localhost:*"))
            );

            app.Use(async (context, next) =>
            {
                string path = context.Request.Path.Value;
                if (path != null && !path.ToLower().Contains("/api"))
                {
                    // XSRF-TOKEN used by angular in the $http if provided
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN",
                      tokens.RequestToken, new CookieOptions
                      {
                          HttpOnly = false,
                          Secure = true,
                          Path = "/"
                      }
                    );
                }

                await next();
            });


            // Allow usage of static files
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Use MVC features (APIs among them)
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            // Use Single Page Application features
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
