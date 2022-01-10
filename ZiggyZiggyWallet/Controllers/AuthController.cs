using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.DTOs.Users;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly IAuthService _authService;
        public AuthController(IAuthService auth, UserManager<AppUser> userManager)
        {
            _userMgr = userManager;
            _authService = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Loginin model)
        {

            var user = await _userMgr.FindByEmailAsync(model.email);
            if (user == null)
            {
                ModelState.AddModelError("Invalid", "Credentials provided bu the user is invalid");
                return BadRequest(Util.BuildResponse<object>(false, "Invalid credentials", ModelState, null));
            }

            // check if user's email is confirmed
            if (await _userMgr.IsEmailConfirmedAsync(user))
            {
                var res = await _authService.Login(model.email, model.Password, model.RememberMe);

                if (!res.status)
                {
                    ModelState.AddModelError("Invalid", "Credentials provided bu thm e user is invalid");
                    return BadRequest(Util.BuildResponse<object>(false, "Invalid credentials", ModelState, null));
                }

                return Ok(Util.BuildResponse(true, "Login is sucessful!", null, res));
            }

            ModelState.AddModelError("Invalid", "User must first confirm email before attempting to login");
            return BadRequest(Util.BuildResponse<object>(false, "Email not confirmed", ModelState, null));
        }
    }
}
