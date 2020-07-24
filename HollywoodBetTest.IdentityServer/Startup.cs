

using IdentityServer4;
using HollywoodBetTest.IdentityServer.Data;
using HollywoodBetTest.IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HollywoodBetTest.Data;
using HollywoodBetTest.Models;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using System;
using IdentityServer4.Services;
using HollywoodBetTest.IdentityServer.Services;
using Serilog;

namespace HollywoodBetTest.IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public string ConnectionString => Configuration.GetConnectionString("DefaultConnection");

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Debug("ConfigureServices start");
            services.AddControllersWithViews();

            services.AddDbContext<HollywoodBetTestContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<HollywoodBetTestUser, IdentityRole>()
                .AddEntityFrameworkStores<HollywoodBetTestContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(HollywoodBetTestContext).GetTypeInfo().Assembly.GetName().Name;


            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            }).AddConfigurationStore(options =>
             {
                 options.ConfigureDbContext = b => b.UseSqlServer(ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
             })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlServer(ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
            }).AddAspNetIdentity<HollywoodBetTestUser>();


            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()));

            services.AddScoped<IProfileService, UserProfileService>();
            Log.Debug("ConfigureServices end");

        }

        public void Configure(IApplicationBuilder app)
        {
            Log.Debug("Configure start");

            // this will do the initial DB population
            bool seed = Configuration.GetSection("Data").GetValue<bool>("SeedSystem");
            if (seed)
                SeedData.InitializeDatabase(app);


            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            Log.Debug("Configure end");

        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("Migration: PersistedGrantDbContext");
                var grantDb = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

                grantDb.Database.Migrate();
                grantDb.SaveChanges();

                Console.WriteLine("Migration: ConfigurationDbContext");

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                context.SaveChanges();
                if (!context.Clients.Any())
                {
                    Console.WriteLine("Seeding: Clients");

                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    Console.WriteLine("Seeding: Identity Resources");

                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    Console.WriteLine("Seeding: API Resources");

                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}