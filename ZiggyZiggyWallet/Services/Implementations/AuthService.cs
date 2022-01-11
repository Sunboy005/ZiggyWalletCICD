using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.Users;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly SignInManager<AppUser> _signinMgr;
        private readonly IJWTServices _jwtService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, IJWTServices jWTService)
        {
            _userMgr = userManager;
            _signinMgr = signinManager;
            _jwtService = jWTService;
        }

        public async Task<LoginCred> Login(string email, string password, bool rememberMe)
        {
            var user = await _userMgr.FindByEmailAsync(email);

            var res = await _signinMgr.PasswordSignInAsync(user, password, rememberMe, false);

            if (user == null || res == null || !res.Succeeded)
            {
                return new LoginCred { status = false };
            }
            
            // get jwt token
            var userRoles = await _userMgr.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, userRoles.ToList());


            return new LoginCred { status = true, Id = user.Id, token = token };

        }
    }
}

