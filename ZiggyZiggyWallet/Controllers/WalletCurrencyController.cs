using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
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

        public WalletCurrencyController(ICurrencyServices currServe,IWalletCurrencyServices walletCurrServe,IWalletServices walletServe, IMapper mapper)
        {
            _mapper = mapper;
            _walletCurrServe = walletCurrServe;
            _walletServe = walletServe;
            _currServe = currServe;
        }
        // GET: api/<WalletCurrencyController>
        [HttpGet("get-balance/{walletAddress}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWalletBalance(string walletAddress)
        {
            if (walletAddress == null)
            {
                ModelState.AddModelError("Not found", "User has no Wallet");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            Wallet walletDetail=new Wallet();
             walletDetail.Id = _walletServe.GetWalletByAddress(walletAddress).Result.Id;
             walletDetail.Name =  _walletServe.GetWalletByAddress(walletAddress).Result.Name;

            var walletId = walletDetail.Id;
            var walletName = walletDetail.Name;
            var mainCurr =await _walletCurrServe.GetMainCurrency(walletId);
            var walletBalance = await _walletCurrServe.GetWalletBalance(walletId);
            

            return Ok(Util.BuildResponse<double>(true, $"{walletName} Wallet Balance ", null, walletBalance));
        }

       


        //// GET api/<WalletCurrencyController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<WalletCurrencyController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<WalletCurrencyController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<WalletCurrencyController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
