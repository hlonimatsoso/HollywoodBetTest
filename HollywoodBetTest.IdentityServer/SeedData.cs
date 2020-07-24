﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using HollywoodBetTest.IdentityServer.Data;
using HollywoodBetTest.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using HollywoodBetTest.Data;
using HollywoodBetTest.Models;
using Microsoft.AspNetCore.Builder;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HollywoodBetTest.IdentityServer
{
    public class SeedData
    {
        public static void InitializeDatabase(IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var grantsContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                var configContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                var hollwoodBetContext = serviceScope.ServiceProvider.GetRequiredService<HollywoodBetTestContext>();
                var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<HollywoodBetTestUser>>();
                var roleMgr = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                // Migrations
                Migrations(grantsContext, configContext, hollwoodBetContext);

                // Sees clients
                Clients(configContext);

                // Seed user resources
                UserResources(configContext);

                // Seed api resources
                ApiResources(configContext);

                // Seed roles
                Roles(hollwoodBetContext, roleMgr);

                // Seed users
                Users(hollwoodBetContext, userMgr, roleMgr);

                //grantsContext.SaveChanges();
                //configContext.SaveChanges();
                //hollwoodBetContext.SaveChanges();

            }

        }

        private static void Users(HollywoodBetTestContext hollwoodBetContext, UserManager<HollywoodBetTestUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            List<Claim> claims;

            foreach (var user in Config.GetUsers())
            {
                claims = Config.GetUserClaims()
                               .FirstOrDefault(x => x.Key.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase)).Value
                               .ToList();

                if (claims == null || claims?.Count == 0)
                    Log.Warning($"No claims found for {user.UserName}");

                var search = userMgr.FindByNameAsync(user.UserName).Result;

                // Only users that dont exist
                if (search == null)
                {
                    var result = userMgr.CreateAsync(user, "Pass123$").Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug($"{user.UserName} created");

                    // Add user to role
                    AddUserToRole(user, userMgr, roleMgr);

                    // set all the claims for a user
                    result = userMgr.AddClaimsAsync(user, claims).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug($"with {claims.Count} claims");
                }

            }

        }

        private static void AddUserToRole(HollywoodBetTestUser user, UserManager<HollywoodBetTestUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            IdentityResult result = null;
            bool roleExists = false;

            switch (user.UserName.ToLower())
            {
                case "alice":
                    roleExists = roleMgr.RoleExistsAsync("CUSTOMER").Result;
                    if (roleExists)
                        result = userMgr.AddToRoleAsync(user, HollywoodBetTest.Models.Roles.Consumer).Result;
                    break;
                case "bob":
                    roleExists = roleMgr.RoleExistsAsync("MANAGER").Result;
                    if (roleExists)
                        result = userMgr.AddToRoleAsync(user, HollywoodBetTest.Models.Roles.Manager).Result;
                    break;
                case "admin":
                    roleExists = roleMgr.RoleExistsAsync("ADMIN").Result;
                    if (roleExists)
                        result = userMgr.AddToRoleAsync(user, HollywoodBetTest.Models.Roles.Admin).Result;
                    break;
                default:
                    break;
            }

            if (result.Succeeded)
            {
                Log.Debug($"{user.UserName} added to role");
            }
            else
            {
                Log.Error($"Addding {user.UserName} to role failed. {result.Errors.FirstOrDefault().Description}");
            }
        }

        private static void Clients(ConfigurationDbContext configContext)
        {
            Console.Write("Seeding: Clients");

            foreach (var client in Config.GetClients())
            {
                if (!configContext.Clients.Any(c => c.ClientName == client.ClientName))
                {
                    configContext.Clients.Add(client.ToEntity());
                }
            }
            configContext.SaveChanges();
            Console.WriteLine("Done");

        }

        private static void UserResources(ConfigurationDbContext configContext)
        {
            Console.Write("Seeding: User Resources...");

            foreach (var resource in Config.GetIdentityResources())
            {
                if (!configContext.IdentityResources.Any(c => c.Name == resource.Name))
                {
                    configContext.IdentityResources.Add(resource.ToEntity());
                }
            }
            configContext.SaveChanges();
            Console.WriteLine("Done");

        }

        private static void Roles(HollywoodBetTestContext configContext, RoleManager<IdentityRole> roleMgr)
        {
            Console.Write("Seeding: Roles...");

            

            //var roleStore = new RoleStore<IdentityRole>(configContext);

            foreach (var role in Config.GetRoles())
            {
                if (!roleMgr.RoleExistsAsync(role.Name).Result)
                {
                    roleMgr.CreateAsync(role).Wait();
                    configContext.SaveChanges();

                }
                //    if (!configContext.Roles.Any(c => c.Name == role.Name))
                //{
                //    configContext.Roles.Add(role);
                //}
            }
            Console.WriteLine("Done");

        }

        private static void ApiResources(ConfigurationDbContext configContext)
        {
            Console.Write("Seeding: API Resources...");

            foreach (var resource in Config.GetApiResources())
            {
                if (!configContext.ApiResources.Any(c => c.Name == resource.Name))
                {
                    configContext.ApiResources.Add(resource.ToEntity());
                }
            }
            configContext.SaveChanges();
            Console.WriteLine("Done");

        }

        private static void Migrations(PersistedGrantDbContext grantsContext, ConfigurationDbContext configContext, HollywoodBetTestContext hollywoodBetContext)
        {
            Console.WriteLine("Migrating grants");

            grantsContext.Database.Migrate();

            Console.WriteLine("Migrating configuration");

            configContext.Database.Migrate();

            Console.WriteLine("Migrating main tables");

            hollywoodBetContext.Database.Migrate();

        }
        //public static void EnsureSeedData(string connectionString)
        //{
        //    var services = new ServiceCollection();
        //    services.AddLogging();
        //    services.AddDbContext<HollywoodBetTestContext>(options =>
        //       options.UseSqlServer(connectionString));

        //    services.AddIdentity<HollywoodBetTestUser, IdentityRole>()
        //        .AddEntityFrameworkStores<HollywoodBetTestContext>()
        //        .AddDefaultTokenProviders();

        //    using (var serviceProvider = services.BuildServiceProvider())
        //    {
        //        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //        {
        //            var context = scope.ServiceProvider.GetService<HollywoodBetTestContext>();
        //            context.Database.Migrate();

        //            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<HollywoodBetTestUser>>();
        //            var alice = userMgr.FindByNameAsync("alice").Result;
        //            if (alice == null)
        //            {
        //                alice = new HollywoodBetTestUser
        //                {
        //                    UserName = "alice",
        //                    Email = "AliceSmith@email.com",
        //                    EmailConfirmed = true,
        //                };
        //                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
        //                if (!result.Succeeded)
        //                {
        //                    throw new Exception(result.Errors.First().Description);
        //                }

        //                result = userMgr.AddClaimsAsync(alice, new Claim[]{
        //                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
        //                    new Claim(JwtClaimTypes.GivenName, "Alice"),
        //                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
        //                    new Claim("api1.read", "true")

        //                }).Result;
        //                if (!result.Succeeded)
        //                {
        //                    throw new Exception(result.Errors.First().Description);
        //                }
        //                Log.Debug("alice created");
        //            }
        //            else
        //            {
        //                Log.Debug("alice already exists");
        //            }

        //            var bob = userMgr.FindByNameAsync("bob").Result;
        //            if (bob == null)
        //            {
        //                bob = new HollywoodBetTestUser
        //                {
        //                    UserName = "bob",
        //                    Email = "BobSmith@email.com",
        //                    EmailConfirmed = true
        //                };
        //                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
        //                if (!result.Succeeded)
        //                {
        //                    throw new Exception(result.Errors.First().Description);
        //                }

        //                result = userMgr.AddClaimsAsync(bob, new Claim[]{
        //                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
        //                    new Claim(JwtClaimTypes.GivenName, "Bob"),
        //                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
        //                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
        //                    new Claim(JwtClaimTypes.Role, Roles.Admin),
        //                    new Claim("api1", "api1.read"),
        //                    new Claim("api", "write"),
        //                    new Claim("location", "somewhere")
        //                }).Result;
        //                if (!result.Succeeded)
        //                {
        //                    throw new Exception(result.Errors.First().Description);
        //                }
        //                Log.Debug("bob created");
        //            }
        //            else
        //            {
        //                Log.Debug("bob already exists");
        //            }
        //        }
        //    }
        //}
    }
}
