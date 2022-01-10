using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.DTOs.Systems;
using ZiggyZiggyWallet.DTOs.Users;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly IWalletCurrencyServices _wallCurServe;
        private readonly IMapper _mapper;

        public UserController(
                               UserManager<AppUser> userManager,
                               RoleManager<IdentityRole> roleMgr,
                                IMapper mapper,
                                IWalletCurrencyServices wallCurrServe)
        {
           
            _userMgr = userManager;
            _roleMgr = roleMgr;
            _mapper = mapper;
            _wallCurServe = wallCurrServe;
        }


        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser(Register model)
        {
            // if user already exist return early
            var existingEmailUser = await _userMgr.FindByEmailAsync(model.Email);
            if (existingEmailUser != null)
            {
                ModelState.AddModelError("Invalid", $"User with email: {model.Email} already exists");
                return BadRequest(Util.BuildResponse<object>(false, "User already exists!", ModelState, null));
            }

            // map data from model to user
            var user = _mapper.Map<AppUser>(model);
            //user.Address.Street = model.Street;
            //user.Address.State = model.State;
            //user.Address.Country = model.Country;


            var response = await _userMgr.CreateAsync(user, model.Password);

            if (!response.Succeeded)
            {
                foreach (var err in response.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<string>(false, "Failed to add user!", ModelState, null));
            }

            var res = await _userMgr.AddToRoleAsync(user, model.Role);

            if (!res.Succeeded)
            {
                foreach (var err in response.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<string>(false, "Failed to add user role!", ModelState, null));
            }

            // if you system requires user's email to be confirmed before they can login 
            // you can generate confirm email token here
            // but ensure AddDefaultTokenProviders() have been enabled in startup else there won't be token generated
            var token = await _userMgr.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("ConfrimEmail", "User", new { email = user.Email, _token = token }, Request.Scheme);  // this is the url to send

            // next thing TODO here is to send an email to this new user to the email provided using a notification service you should build

            // map data to dto
            var details = _mapper.Map<RegisterSuccess>(user);

            // the confirmation link is added to this response object for testing purpose since at this point it is not being sent via mail
            return Ok(Util.BuildResponse(true, "New user added!", null, new { details, ConfimationLink = url }));


        }

        [Authorize]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfrimEmail(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("Invalid", "UserId and token is required");
                return BadRequest(Util.BuildResponse<object>(false, "UserId or token is empty!", ModelState, null));
            }

            var user = await _userMgr.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("NotFound", $"User with email: {email} was not found");
                return NotFound(Util.BuildResponse<object>(false, "User not found!", ModelState, null));
            }

            var res = await _userMgr.ConfirmEmailAsync(user, token);
            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<object>(false, "Failed to confirm email", ModelState, null));
            }

            return Ok(Util.BuildResponse<object>(true, "Email confirmation suceeded!", null, null));
        }


        [HttpGet("get-users")]
        [AllowAnonymous]
        public IActionResult GetUsers(int page, int perPage)
        {
            // map data from db to dto to reshape it and remove null fields
            var listOfUsersToReturn = new List<UserToReturn>();

            //var users = _userService.Users;
            var users = _userMgr.Users.ToList();

            if (users != null)
            {
                var pagedList = PagedList<AppUser>.Paginate(users, page, perPage);
                foreach (var user in pagedList.Data)
                {
                    listOfUsersToReturn.Add(_mapper.Map<UserToReturn>(user));
                }

                var res = new PaginatedList<UserToReturn>
                {
                    MetaData = pagedList.MetaData,
                    Data = listOfUsersToReturn
                };

                return Ok(Util.BuildResponse(true, "List of users", null, res));
            }
            else
            {
                ModelState.AddModelError("Notfound", "There was no record for users found!");
                var res = Util.BuildResponse<List<UserToReturn>>(false, "No results found!", ModelState, null);
                return NotFound(res);
            }

        }

        [HttpGet("get-user/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            // map data from db to dto to reshape it and remove null fields

            var user = await _userMgr.FindByEmailAsync(email);
            if (user != null)
            {
                var userToReturn = new UserToReturn
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                var res = Util.BuildResponse(true, "User details", null, userToReturn);
                return Ok(res);
            }
            else
            {
                ModelState.AddModelError("Notfound", $"There was no record found for user with email {user.Email}");
                return NotFound(Util.BuildResponse<List<UserToReturn>>(false, "No result found!", ModelState, null));
            }

        }


        [HttpGet("get-user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByID(string userId)
        {
            // map data from db to dto to reshape it and remove null fields

            var user = await _userMgr.FindByIdAsync(userId);
            if (user != null)
            {
                var userToReturn = new UserToReturn
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                var res = Util.BuildResponse(true, "User details", null, userToReturn);
                return Ok(res);
            }
            else
            {
                ModelState.AddModelError("Notfound", $"There was no record found for user with User Id {user.Id}");
                return NotFound(Util.BuildResponse<List<UserToReturn>>(false, "No result found!", ModelState, null));
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-user-type")]
        public async Task<IActionResult> UpgradeUser(string userId, string roleToAdd)
        {
            //Check If User exist
            var user = await _userMgr.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("NotFound", $"User with id: {userId} was not found");
                return NotFound(Util.BuildResponse<object>(false, "User not found!", ModelState, null));
            }

            var rolesAvail = new List<string>();
            var rolesAvailable = _roleMgr.Roles.ToList();
            foreach (var roleavail in rolesAvailable)
            {
                rolesAvail.Add(roleavail.Name);
            }
            if (!rolesAvail.Contains(roleToAdd))
            {
                ModelState.AddModelError("NotFound", $"The inputed role {roleToAdd} is not available for use");
                return NotFound(Util.BuildResponse<object>(false, "Role not found!", ModelState, null));
            }
            var currentRoles = await _userMgr.GetRolesAsync(user);

            if (currentRoles.Contains(roleToAdd))
            {
                ModelState.AddModelError("NotFound", $"User is a/an {roleToAdd} user");
                return NotFound(Util.BuildResponse<object>(false, "User role not Updated!", ModelState, null));
            }
            if (currentRoles.Contains("Admin"))
            {
                ModelState.AddModelError("NotFound", $"Admin cannot be assigned with either Noob or Elite Roles {roleToAdd}");
                return NotFound(Util.BuildResponse<object>(false, "User role not Updated!", ModelState, null));
            }

            if (roleToAdd == "Elite")
            {
                //Remove the Role
                await _userMgr.RemoveFromRolesAsync(user, currentRoles);
                await _userMgr.AddToRoleAsync(user, "Elite");
                return NotFound(Util.BuildResponse<object>(false, "Wallet Merge Error!", null, $"User has being Upgraded to { roleToAdd}"));
            }
            else
            {
                //Perform Merging of Currencies and wallets
                var wallMerge = await _wallCurServe.MergeWallets(userId);
                if (wallMerge == true)
                {
                    //Remove the Role
                    await _userMgr.RemoveFromRolesAsync(user, currentRoles);

                    //Add a new role 
                    await _userMgr.AddToRoleAsync(user, "Noob");
                    return Ok(Util.BuildResponse<object>(true, "Wallet Merge Error!", null, $"User has being downgraded to { roleToAdd}"));
                }
            }
            ModelState.AddModelError("NotFound", $"Wallet Merge Failed");
            return NotFound(Util.BuildResponse<object>(false, "User role not Updated!", ModelState, null));

        }
    }
}
