using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HollywoodBetTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;

namespace HollywoodBetTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly SignInManager<HollywoodBetTestUser> _signInManager;
        private readonly UserManager<HollywoodBetTestUser> _userManager;

        public AccountsController(UserManager<HollywoodBetTestUser> _manager)
        {
            this._userManager = _manager;
        }

        public async Task<IActionResult> OnPostAsync(LocalRegisterModel data)
        {
            if (ModelState.IsValid)
            {
                var user = new HollywoodBetTestUser { UserName = data.Email };

                if (ModelState.IsValid)
                {
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {

                    }

                }

            }
            return Ok();
        }
    }
}
