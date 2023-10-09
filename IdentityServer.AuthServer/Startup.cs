using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repository;
using IdentityServer.AuthServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer
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
            services.AddDbContext<CustomDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("LocalDb"));
            });
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddIdentityServer().AddConfigurationStore(opt =>
            {
                opt.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("LocalDb"), sqlopt =>
                {
                    sqlopt.MigrationsAssembly(assemblyName);
                });
            })
            .AddOperationalStore(opts =>
            {
                opts.ConfigureDbContext = c => c.UseSqlServer(Configuration.GetConnectionString("LocalDb"), sqlopt =>
                {
                    sqlopt.MigrationsAssembly(assemblyName);
                });
            })
            //.AddInMemoryApiResources(Config.GetApiResource()).AddInMemoryApiScopes(Config.GetApiScopes())
            //.AddInMemoryClients(Config.GetClient()).AddInMemoryIdentityResources(Config.GetIdentityResources())
            //.AddTestUsers(Config.GetUsers().ToList());
            .AddDeveloperSigningCredential()
            .AddProfileService<CustomProfileService>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
