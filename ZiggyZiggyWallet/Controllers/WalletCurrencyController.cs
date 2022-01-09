using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZiggyZiggyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletCurrencyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalletCurrencyServices _walletCurrServe;
        private readonly IWalletServices _walletServe;
        private readonly ICurrencyServices _currServe;
        private readonly UserManager<AppUser> _userMgr;


        public WalletCurrencyController(ICurrencyServices currServe, IWalletCurrencyServices walletCurrServe, IWalletServices walletServe, IMapper mapper, UserManager<AppUser> userMgr)
        {
            _mapper = mapper;
            _walletCurrServe = walletCurrServe;
            _walletServe = walletServe;
            _currServe = currServe;
            _userMgr = userMgr;
        }
        // GET: api/<WalletCurrencyController>
        [Authorize]
        [HttpGet("get-balance/{walletAddress}")]
        public async Task<IActionResult> GetWalletBalance(string walletAddress)
        {
            if (walletAddress == null)
            {
                ModelState.AddModelError("Not found", "User has no Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            Wallet walletDetail = new Wallet();
            walletDetail.Id = _walletServe.GetWalletByAddress(walletAddress).Result.Id;
            walletDetail.Name = _walletServe.GetWalletByAddress(walletAddress).Result.Name;

            var walletId = walletDetail.Id;
            var walletName = walletDetail.Name;
            var mainCurr = await _walletCurrServe.GetMainCurrency(walletId);
            var walletBalance = await _walletCurrServe.GetWalletBalance(walletId);


            return Ok(Util.BuildResponse<double>(true, $"{walletName} Wallet Balance ", null, walletBalance));
        }
        // GET: api/<WalletCurrencyController>
        [Authorize]
        [HttpGet("get-currency-list/{walletAddress}")]
        public async Task<IActionResult> GetCurrencyList(string walletAddress)
        {
            //Check if Wallet Exist
            var walletDetail = await _walletServe.GetWalletByAddress(walletAddress);
            if (walletDetail == null)
            {
                ModelState.AddModelError("Not found", "No Wallet with such Address");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            //If Yes Wallet Details
            var walletName = walletDetail.Name;
            var walletId = walletDetail.Id;

            //List all the Currencies
            var currencyList = await _walletCurrServe.ListAllCurrencies(walletId);
            if (currencyList == null)
            {
                ModelState.AddModelError("Not found", $"No Currency found for {walletName}'s wallet transactions");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }

            // map result
            var listOfCurrToReturn = new List<WalletCurrencyToReturn>();
            foreach (var wallCurrency in currencyList)
            {
                var wallCurrToReturn = _mapper.Map<WalletCurrencyToReturn>(wallCurrency);
                
                wallCurrToReturn.Name = walletName;
                listOfCurrToReturn.Add(wallCurrToReturn);

            }

            return Ok(Util.BuildResponse<List<WalletCurrencyToReturn>>(true, $"List of Currencies in {walletName}  ", null, listOfCurrToReturn));
        }
        ////ec96ab45-8801-49ca-b0d6-379cd4eb4d3f

        // GET: api/<WalletCurrencyController>
        [Authorize(Roles = "Admin,Noob,Elite")]
        [HttpDelete("delete-a-currency")]
        public async Task<IActionResult> DeleteACurrency(string currencyId, string walletAddress)
        {
            //Check if Wallet Exist
            var walletDetail = await _walletServe.GetWalletByAddress(walletAddress);
            if (walletDetail == null)
            {
                ModelState.AddModelError("Not found", "User has no Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            //If Yes Wallet Details
            var walletName = walletDetail.Name;
            var walletId = walletDetail.Id;


            //Check for Currency
            var wallCurrExist = await _walletCurrServe.CheckCurrencyInWallet(walletId, currencyId);
            if (!wallCurrExist)
            {
                ModelState.AddModelError("Not found", "User has no currency in the Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            //Get Main Currency Id
            var getMain = await _walletCurrServe.GetMainCurrency(walletId);
            var mainCurId = getMain.Id;

            //Check if the Currency to Delete is not main
            if (currencyId == mainCurId)
            {
                ModelState.AddModelError("Not found", "You can't Delete your mainCurrency");
                return NotFound(Util.BuildResponse<object>(false, "Failed to Delete", ModelState, null));
            }

            //if Currency Exist in a Wallet Curr Details
            var currRem = await _walletCurrServe.RemoveACurrency(currencyId, walletId);


            return Ok(Util.BuildResponse<string>(true, $"{walletName} has been updated ", null, currRem.Item2));
        }

        // GET: api/<WalletCurrencyController>
        [Authorize(Roles ="Admin,Noob,Elite")]
        [HttpDelete("delete-currencies")]
        public async Task<IActionResult> DeleteAllCurrency(string walletAddress)
        {
            //Check if Wallet Exist
            var walletDetail = await _walletServe.GetWalletByAddress(walletAddress);
            if (walletDetail == null)
            {
                ModelState.AddModelError("Not found", "User has no Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            //If Yes Wallet Details
            var walletName = walletDetail.Name;
            var walletId = walletDetail.Id;



            //if Currency Exist in a Wallet Curr Details
            var currRem = await _walletCurrServe.RemoveAllCurrency(walletId);


            return Ok(Util.BuildResponse<string>(true, $"{walletName} has been updated ", null, currRem.Item2));
        }

    }
}
