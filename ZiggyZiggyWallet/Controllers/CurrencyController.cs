using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.DTOs.Systems;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyServices _curServe;
        private readonly IMapper _mapper;

        public CurrencyController(ICurrencyServices curServe, IMapper mapper)
        {
            _curServe = curServe;
            _mapper = mapper;
        }

        //[Authorize(Roles="Admin")]
        [HttpPost("add-currency")]
        public async Task<ActionResult<Currency>> AddCurrency(CurrencyToAdd model)
        {
            //check if user logged is the one making the changes - only works for system using Auth tokens
            ClaimsPrincipal currentUser = this.User;
            var currentRole = currentUser.FindFirst(ClaimTypes.Role).Value;

            if (currentRole == "Admin")
            {
                ModelState.AddModelError("Denied", "You are not permitted to access this feature");
                var result2 = Util.BuildResponse<string>(false, "Access denied!", ModelState, null);
                return BadRequest(result2);
            }

            // Map DTO to User
            var currency = new Currency
            {
                Name = model.Name,
                Abbrevation = model.Abbrevation,
                ShortCode = model.ShortCode
            };

            try
            {
                var response = await _curServe.AddCurrency(model);
                if (!response.Item1)
                {
                    ModelState.AddModelError("Failed", "Could not add New Currency to database");
                    return BadRequest(Util.BuildResponse<object>(false, "Failed to add to database", ModelState, null));
                }

                return Ok(Util.BuildResponse<Currency>(true, "New Currency successfully", null, currency));

            }
            catch (DbException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        [HttpGet("get-currency/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrencyById(string id)
        {
            var currency = await _curServe.GetCurrencyById(id);
            if (currency == null)
            {
                ModelState.AddModelError("Not found", $"No Currency with this Id {id}");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            var CurrencyToReturn = _mapper.Map<CurrencyToReturn>(currency);

            return Ok(Util.BuildResponse<CurrencyToReturn>(true, $"Currencies with id of {id}", null, CurrencyToReturn));
        }

        [HttpGet("get-all-Currencies")]
        public async Task<IActionResult> GetAllCurrencies(int page, int perPage)
        {
            var currencies = await _curServe.GetAllCurrency();
            if (currencies == null)
            {
                ModelState.AddModelError("Not found", "No result found for currencies");
                return NotFound(Util.BuildResponse<object>(false, "Result is empty", ModelState, null));
            }
            var pagedList = PagedList<Currency>.Paginate(currencies, page, perPage);


            // map result
            var listOfCurrenciesToReturn = new List<CurrencyToReturn>();
            foreach (var currency in currencies)
            {
                var currencyToReturn = _mapper.Map<CurrencyToReturn>(currency);
                listOfCurrenciesToReturn.Add(currencyToReturn);
            }
            var res = new PaginatedList<CurrencyToReturn>
            {
                MetaData = pagedList.MetaData,
                Data = listOfCurrenciesToReturn
            };
            return Ok(Util.BuildResponse<object>(true, "List of users currencies", null, listOfCurrenciesToReturn));
        }

    }
}
