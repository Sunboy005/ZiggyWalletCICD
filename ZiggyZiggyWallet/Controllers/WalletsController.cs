using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Systems;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletServices _walletServe;
        private readonly IMapper _mapper;

        public WalletsController(IWalletServices walletServe, IMapper mapper)
        {
            _walletServe = walletServe;
            _mapper = mapper;
        }
        [Authorize(Roles ="Noob, Elite")]
        // GET: api/Wallets
        [HttpPost("add-wallet")]
        public async Task<ActionResult<Wallet>> AddAWallet(WalletToAdd model, string userId)
        {
            //check if user logged is the one making the changes - only works for system using Auth tokens
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!userId.Equals(currentUserId))
            {
                ModelState.AddModelError("Denied", $"You are not allowed to Add Wallet for another user");
                var result2 = Util.BuildResponse<string>(false, "Access denied!", ModelState, "");
                return BadRequest(result2);
            }
            var walletList = _walletServe.GetAllUsersWalletsList(userId).Result;
            var walletCount = walletList.Count();
            var currentRole = currentUser.FindFirst(ClaimTypes.Role).Value;

            if (currentRole == "Noob" && walletCount >= 1)
            {
                ModelState.AddModelError("Denied", $"You are not permitted to Add More than one wallet, Contact Admin to Upgrade");
                var result2 = Util.BuildResponse<string>(false, "Access denied!", ModelState, "");
                return BadRequest(result2);
            }

            // Map DTO to User
            var wallet = new Wallet
            {
                Name = model.Name,
                Address = model.Address,
                IsMain = model.IsMain
            };

            try
            {
                var response = await _walletServe.AddAWallet(model, userId);
                if (!response.Item1)
                {
                    ModelState.AddModelError("Failed", "Could not add New wallet to database");
                    return BadRequest(Util.BuildResponse<object>(false, "Failed to add to database", ModelState, null));
                }

                return Ok(Util.BuildResponse<Wallet>(true, "New Wallet successfully", null, wallet));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // GET: Wallet by Address
        [HttpGet("get-main/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserMainWallet(string userId)
        {
            var wallets = await _walletServe.GetAllUsersWalletsList(userId);
            if (wallets == null)
            {
                ModelState.AddModelError("Not found", "User has no Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            var mainWallet = wallets.FirstOrDefault(x => x.IsMain == true && x.AppUserId == userId);
            if (mainWallet == null)
            {
                 ModelState.AddModelError("Not found", "User has no Main Wallet");
            return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            var walletToReturn = _mapper.Map<WalletToReturn>(mainWallet);

            return Ok(Util.BuildResponse<WalletToReturn>(true, "User's Main Wallet", null, walletToReturn));
        }

        // GET: Wallet by Id
        [HttpGet("get-wallet/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserWallet(string id)
        {
            var wallet = await _walletServe.GetWalletbyId(id);
            if (wallet == null)
            {
                ModelState.AddModelError("Not found", "User has no Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            var walletToReturn = _mapper.Map<WalletToReturn>(wallet);

            return Ok(Util.BuildResponse<WalletToReturn>(true, "User's Main Wallet", null, walletToReturn));
        }

        // GET: All Wallet by Id
        [Authorize(Roles ="Admin")]
        [HttpGet("get-all-wallets")]
        public async Task<IActionResult> GetAllWallets(int page, int perPage)
        {
            var wallets = await _walletServe.GetAllWalletLists();
            if (wallets == null)
            {
                ModelState.AddModelError("Not found", "No result found for wallets");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            var pagedList = PagedList<Wallet>.Paginate(wallets, page, perPage);
            
            
            // map result
            var listOfWalletsToReturn = new List<WalletToReturn>();
            foreach (var wallet in wallets)
            {
                var walletToReturn = _mapper.Map<WalletToReturn>(wallet);
                listOfWalletsToReturn.Add(walletToReturn);
            }
            var res = new PaginatedList<WalletToReturn>
            {
                MetaData = pagedList.MetaData,
                Data = listOfWalletsToReturn
            };
            return Ok(Util.BuildResponse<object>(true, "List of users wallets", null, listOfWalletsToReturn));
        }


        // GET: Main Wallet
        [HttpGet("get-wallets-list/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserWalletList(string userId)
        {
            var wallets = await _walletServe.GetAllUsersWalletsList(userId);
            if (wallets == null)
            {
                ModelState.AddModelError("Not found", "No result found for wallets");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }

            // map result
            var listOfWalletsToReturn = new List<WalletToReturn>();
            foreach (var wallet in wallets)
            {
                var walletToReturn = _mapper.Map<WalletToReturn>(wallet);
                listOfWalletsToReturn.Add(walletToReturn);

            }

            return Ok(Util.BuildResponse<List<WalletToReturn>>(true, "List of user's photos", null, listOfWalletsToReturn));
        }
    }
}
