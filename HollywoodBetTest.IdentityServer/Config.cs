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
                new IdentityResource("role",new List<string>{"role" }),
                new IdentityResource("custom","Extra User Info",new List<string>{"nickName", "religion" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("tournaments", "Access to tournaments")
                {
                    UserClaims =  new List<string> {
                        "tournaments.read",
                        "tournaments.write",
                        "tournaments.delete"
                    },
                    Scopes =  new List<string> { "tournaments" }
                },
                new ApiResource("events", "Access to events")
                {
                    UserClaims =  new List<string> {
                        "events.read",
                        "events.write",
                        "events.delete"
                    },
                    Scopes =  new List<string> { "events" }
                },
                new ApiResource("horses", "Access to horses")
                {
                    UserClaims =  new List<string> {
                        "horses.read",
                        "horses.write",
                        "horses.delete"
                    },
                    Scopes =  new List<string> { "horses" }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
            new ApiScope("tournaments", "Access to tournaments"),
            new ApiScope("events", "Access to events"),
            new ApiScope("horses", "Access to horses")

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
                    AllowedScopes = { "openid", "profile", "email","role","custom", "tournaments", "events","horses" },
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
               new IdentityRole(Roles.Admin),new IdentityRole(Roles.Customer),new IdentityRole(Roles.Manager)
            };
        }

        internal static IEnumerable<string> GetEventDetailStatus()
        {
            return new List<string> {"Active","In-Active"};
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
                new Claim(JwtClaimTypes.FamilyName, "The Oracles"),
                new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                new Claim("role", Roles.Admin),
                new Claim("nickName", "The BOSS"),
                new Claim("religion", "Nation Of Islam"),
                new Claim("tournaments.read", "true"),
                new Claim("tournaments.write", "true"),
                new Claim("tournaments.delete", "true"),
                new Claim("events.read", "true"),
                new Claim("events.write", "true"),
                new Claim("events.delete", "true"),
                new Claim("horses.read", "true"),
                new Claim("horses.write", "true"),
                new Claim("horses.delete", "true")

            });

            retVal.Add("alice", new List<Claim> {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alicesmith.com"),
                new Claim("Role", Roles.Customer),
                new Claim("nickName", "Nice Alice"),
                new Claim("religion", "Christian"),
                new Claim("tournaments.read", "true"),
                new Claim("events.read", "true"),
                new Claim("horses.read", "true")
            });

            retVal.Add("bob", new List<Claim> {
                new Claim(JwtClaimTypes.Name, "Bob SMith"),
                new Claim(JwtClaimTypes.GivenName, "Bob"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://bravo.com"),
                new Claim("role", Roles.Manager),
                new Claim("nickName", "Jonny Bravo"),
                new Claim("religion", "Jewish"),
                new Claim("tournaments.read", "true"),
                new Claim("tournaments.write", "true"),
                new Claim("events.read", "true"),
                new Claim("events.write", "true"),
                new Claim("horses.read", "true"),
                new Claim("horses.write", "true")

            });
            return retVal;
        }
    }
}