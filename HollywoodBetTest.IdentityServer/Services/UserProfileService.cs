using HollywoodBetTest.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HollywoodBetTest.IdentityServer.Services
{
  

    public class UserProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<HollywoodBetTestUser> _claimsFactory;
        private readonly UserManager<HollywoodBetTestUser> _userManager;

        public UserProfileService(UserManager<HollywoodBetTestUser> userManager, IUserClaimsPrincipalFactory<HollywoodBetTestUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.Identity.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            claims.Add(new Claim("kasi", user.City ?? string.Empty));
            claims.Add(new Claim("age", user.Age.ToString()));


            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
