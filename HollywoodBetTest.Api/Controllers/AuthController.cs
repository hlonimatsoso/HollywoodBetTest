using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HollywoodBetTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HollywoodBetTest.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            UserAccess access = new UserAccess { };

            foreach (Claim claim in User.Claims)
            {
                if (claim.Type.Equals(User_Access.TOURNAMENT_READ))
                    access.tournaments_read = true;

                if (claim.Type.Equals(User_Access.TOURNAMENT_WRITE))
                    access.tournaments_write = true;

                if (claim.Type.Equals(User_Access.TOURNAMENT_DELETE))
                    access.tournaments_delete = true;

                if (claim.Type.Equals(User_Access.EVENT_READ))
                    access.events_read = true;

                if (claim.Type.Equals(User_Access.EVENT_WRITE))
                    access.events_write = true;

                if (claim.Type.Equals(User_Access.EVENT_DELETE))
                    access.events_delete = true;

                if (claim.Type.Equals(User_Access.HORSE_READ))
                    access.horses_read = true;

                if (claim.Type.Equals(User_Access.HORSE_WRITE))
                    access.horses_write = true;

                if (claim.Type.Equals(User_Access.HORSE_DELETE))
                    access.horses_delete = true;
            }
                     

            return new JsonResult(access);
        }
    }
}
