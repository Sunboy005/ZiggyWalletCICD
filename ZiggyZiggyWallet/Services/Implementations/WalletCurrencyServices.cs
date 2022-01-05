using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs.WalletCurrency;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class WalletCurrencyServices : IWalletCurrencyServices
    {
        private readonly IWalletCurrencyRepository _walletCurRepo;
        private readonly ICurrencyRepository _curRepo;
        private readonly IMapper _mapper;

        public WalletCurrencyServices(IWalletCurrencyRepository walletCurRepo, IMapper mapper, IWalletServices walletServe, ICurrencyRepository curRepo, UserManager<AppUser> userMng)
        {
            _walletCurRepo = walletCurRepo;
            _curRepo = curRepo;
            _mapper = mapper;

        }

        public async Task<Tuple<bool, float>> ConvertCurrencyToBaseCurrency(string walletId, string baseCurrencyId, string fromCurrencyId)
        {            
             //Fetching details of all Currencies involved          
             var baseCurr = await _walletCurRepo.GetWalletCurrencyDetails(walletId, baseCurrencyId);
             //var fromCurr = await _walletCurRepo.GetWalletCurrencyDetails(walletId, fromCurrencyId);
            
            //setting Balance of the Currency
            //var fromBalance =fromCurr.Balance;

            //Get base CurrId
            var baseCurrId = baseCurr.CurrencyId;

            //Fetching Abbrevation to perform Conversion
            var baseCurrencyDetails = await _curRepo.GetCurrencyfromId(baseCurrId);
            var fromCurrencyDetails = await _curRepo.GetCurrencyfromId(fromCurrencyId);
            var from = fromCurrencyDetails.Abbrevation;
            var to = baseCurrencyDetails.Abbrevation;

            //Get Currency for Updating
            var convertingToBase = CurrencyConverter.GetExchangeRate(from, to, 1);

            var balanceToAdd= convertingToBase * 1 ;
            //baseCurr.Balance+= balanceToAdd;
            //fromCurr.Balance-= fromBalance;
            var newbaseBal = (float)balanceToAdd;
            
            return  new Tuple<bool, float>(true,newbaseBal);
        }

        public async Task<Tuple<bool, float>> ConvertCurrencyToCurrency(string toCurrencyId, string fromCurrencyId)
        {
            //Fetching Abbrevation to perform Conversion
            var toCurrencyDetails = await _curRepo.GetCurrencyfromId(toCurrencyId);
            var fromCurrencyDetails = await _curRepo.GetCurrencyfromId(fromCurrencyId);
            var from = fromCurrencyDetails.Abbrevation;
            var to = toCurrencyDetails.Abbrevation;

            //Get Currency for Updating
            var converterRate = CurrencyConverter.GetExchangeRate(from, to, 1);
            //var typi= typeof(converterRate);
            var balanceToAdd= converterRate * 1 ;
            var newbaseBal = (float)balanceToAdd;
            
            return  new Tuple<bool, float>(true,newbaseBal);
        }


        public async Task<float> GetCurrencyBalance(string walletId, string currencyId)
        {
            var res = await _walletCurRepo.GetWalletCurrencyDetails(walletId, currencyId);
            return res.Balance;
        }

        //Check if Currency Exist in a Wallet
        public async Task<bool> CheckCurrencyInWallet(string walletId, string currencyId)
        {
            var walletCurrencyList = await _walletCurRepo.GetCurrenciesListInAWallet(walletId);
            for (int i = 0; i < walletCurrencyList.Count; i++)
            {
                if(currencyId==walletCurrencyList[i].CurrencyId)
                    return true;
            }
            return false;
        }

        //Get Main Currency in a Wallet 
        public async Task<WalletCurrency> GetMainCurrency(string walletId)
        {
            var res = await _walletCurRepo.GetMainCurrency(walletId);

            //var currency = await _curRepo.GetCurrencyfromId(res);
            return res;
        }

        public async Task<double> GetWalletBalance(string walletid)
        {
            var res = await _walletCurRepo.GetCurrenciesListInAWallet(walletid);
            var mainCur = await _walletCurRepo.GetMainCurrency(walletid);
            var walletBalance = mainCur.Balance;

            foreach (var item in res)
            {

                //Convert each currency to main
                var convBal = 0.00F;
                if (mainCur.CurrencyId != item.CurrencyId)
                {
                    var ressy = await ConvertCurrencyToBaseCurrency(walletid, mainCur.CurrencyId, item.CurrencyId);
                    
                    convBal = (float)ressy.Item2;
                }
                walletBalance += convBal;
            }
            return walletBalance;
        }

        public async Task<Tuple<bool, WalletCurrencyToAdd>> AddACurrency(WalletCurrencyToAdd model, string walletId)
        {
            var mainCurExist = await _walletCurRepo.GetMainCurrency(walletId);

            var walletCur = _mapper.Map<WalletCurrency>(model);
            walletCur.CurrencyId= model.CurrencyId;
            walletCur.Balance= model.Balance;
            walletCur.IsMain= model.IsMain;
            walletCur.WalletId= model.WalletId;

            if (mainCurExist==null)
                walletCur.IsMain = true;

            var res = await _walletCurRepo.Add(walletCur);

            return new Tuple<bool, WalletCurrencyToAdd>(res, model);
        }
    }
}
