// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using HollywoodBetTest.Models;
using IdentityModel;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace HollywoodBetTest.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource("Role",new List<string>{Roles.Admin,Roles.Consumer, Roles.Manager }),
                new IdentityResource("Custom","Extra User Info",new List<string>{"nickName", "age","kasi" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "Resource 1 API")
                {
                    Scopes =  new List<string> { "api1.read", "api1.write", "api1.delete" }
                },
                new ApiResource("api2", "Resource 2 API")
                {
                    Scopes =  new List<string> { "api2.read", "api2.write", "api2.delete" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "hollywoodbet",
                    ClientName = "Hollywood Bet Test Angular App",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email","role","custom", "api1", "api2" },
                    RedirectUris = {"http://localhost:12000/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:12000/"},
                    AllowedCorsOrigins = {"http://localhost:12000"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                }
            };
        }

        internal static IEnumerable<IdentityRole> GetRoles()
        {
            return new List<IdentityRole> {
               new IdentityRole(Roles.Admin),new IdentityRole(Roles.Consumer),new IdentityRole(Roles.Manager)
            };
        }

        public static IEnumerable<HollywoodBetTestUser> GetUsers()
        {
            var retVal = new List<HollywoodBetTestUser> { };
            retVal.Add(new HollywoodBetTestUser
            {
                UserName = "admin",
                Email = "admin@test.com",
                EmailConfirmed = true,
                Age = 100,
                City = "Babylon"
            });

            retVal.Add(new HollywoodBetTestUser
            {
                UserName = "alice",
                Email = "alice@test.com",
                EmailConfirmed = true,
                Age = 25,
                City = "Menisoda"

            });

            retVal.Add(new HollywoodBetTestUser
            {
                UserName = "bob",
                Email = "bob@test.com",
                EmailConfirmed = true,
                Age = 45,
                City = "Soweto"
            });

            return retVal;
        }

        public static Dictionary<string, IEnumerable<Claim>> GetUserClaims()
        {
            var retVal = new Dictionary<string, IEnumerable<Claim>> { };
            retVal.Add("admin", new List<Claim> {
                new Claim(JwtClaimTypes.Name, "Administrator"),
                new Claim(JwtClaimTypes.GivenName, "Admin"),
                new Claim(JwtClaimTypes.FamilyName, "The Oracle"),
                new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                new Claim("Role", Roles.Admin),
                new Claim("Age", "33"),
                new Claim("City", "JHB"),
                new Claim("api1.read", "true"),
                new Claim("api1.write", "true"),
                new Claim("api1.delete", "true"),
                new Claim("api2.read", "true"),
                new Claim("api2.write", "true"),
                new Claim("api2.delete", "true")

            });

            retVal.Add("alice", new List<Claim> {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alicesmith.com"),
                new Claim("Role", Roles.Consumer),
                new Claim("Customer", "true"),
                new Claim("Age", "25"),
                new Claim("City", "PTA"),
                new Claim("api1.read", "true"),
                new Claim("api2.read", "true")
            });

            retVal.Add("bob", new List<Claim> {
                new Claim(JwtClaimTypes.Name, "Johnny Bravo"),
                new Claim(JwtClaimTypes.GivenName, "Johnny"),
                new Claim(JwtClaimTypes.FamilyName, "Bravo"),
                new Claim(JwtClaimTypes.WebSite, "http://bravo.com"),
                new Claim("Role", Roles.Manager),
                new Claim("Age", "40"),
                new Claim("City", "JHB"),
                new Claim("api1.read", "true"),
                new Claim("api1.write", "true"),
                new Claim("api2.read", "true"),
                new Claim("api2.write", "true")

            });
            return retVal;
        }
    }
}