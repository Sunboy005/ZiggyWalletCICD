using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Commons;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.WalletCurrency;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class WalletCurrencyServices : IWalletCurrencyServices
    {
        private readonly IWalletCurrencyRepository _walletCurRepo;
        private readonly IWalletServices _walletServe;
        private readonly ICurrencyRepository _curRepo;
        private readonly IMapper _mapper;

        public WalletCurrencyServices(IWalletCurrencyRepository walletCurRepo, IMapper mapper, IWalletServices walletServe, ICurrencyRepository curRepo, UserManager<AppUser> userMng)
        {
            _walletCurRepo = walletCurRepo;
            _curRepo = curRepo;
            _mapper = mapper;
            _walletServe = walletServe;

        }

        public async Task<Tuple<bool, float>> ConvertCurrencyToBaseCurrency(string walletId, string baseCurrencyId, string fromCurrencyId)
        {
            //Fetching details of all Currencies involved          
            var baseCurr = await _walletCurRepo.GetWalletCurrencyDetails(walletId, baseCurrencyId);
            var fromCurr = await _walletCurRepo.GetWalletCurrencyDetails(walletId, fromCurrencyId);

            //setting Balance of the Currency
            var fromBalance = fromCurr.Balance;

            //Get base CurrId
            var baseCurrId = baseCurr.CurrencyId;
            var baseCurrbal = baseCurr.Balance;

            //Fetching Abbrevation to perform Conversion
            var baseCurrencyDetails = await _curRepo.GetCurrencyfromId(baseCurrId);
            var fromCurrencyDetails = await _curRepo.GetCurrencyfromId(fromCurrencyId);
            var from = fromCurrencyDetails.Abbrevation;
            var to = baseCurrencyDetails.Abbrevation;

            //Get Currency for Updating
            var convertingToBase = CurrencyConverter.GetExchangeRate(from, to, 1);

            var balanceToAdd = convertingToBase * fromBalance;
            baseCurr.Balance += balanceToAdd;
            fromCurr.Balance -= fromBalance;

            var updateBaseCurr = await _walletCurRepo.Edit(baseCurr);
            var updateCurr = await _walletCurRepo.Edit(fromCurr);


            var newbaseBal = (float)balanceToAdd;

            return new Tuple<bool, float>(true, newbaseBal);
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
            var balanceToAdd = converterRate * 1;
            var newbaseBal = (float)balanceToAdd;

            return new Tuple<bool, float>(true, newbaseBal);
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
                if (currencyId == walletCurrencyList[i].CurrencyId)
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
            walletCur.CurrencyId = model.CurrencyId;
            walletCur.Balance = model.Balance;
            walletCur.IsMain = model.IsMain;
            walletCur.WalletId = model.WalletId;

            if (mainCurExist == null)
                walletCur.IsMain = true;

            var res = await _walletCurRepo.Add(walletCur);

            return new Tuple<bool, WalletCurrencyToAdd>(res, model);
        }

        public async Task<Tuple<bool, string>> RemoveACurrency(string currId, string wallId)
        {
            var mainCurr = await _walletCurRepo.GetMainCurrency(wallId);
            var mainCurrId = mainCurr.CurrencyId;

            var currDetail = await _walletCurRepo.GetWalCurByCurrId(currId);

            //Get the all Currency in a wallet
            if (mainCurrId != currId)
            {
                var curConv = await ConvertCurrencyToBaseCurrency(wallId, mainCurrId, currId);
                if (curConv != null)
                {
                    var curDel = _walletCurRepo.Delete(currDetail);
                }
                return new Tuple<bool, string>(true, $"Currency Deleted");
            }
            return null;
        }


        public async Task<Tuple<bool, string>> RemoveAllCurrency(string wallId)
        {
            var mainCurr = await _walletCurRepo.GetMainCurrency(wallId);
            var mainCurrId = mainCurr.CurrencyId;

            //Get the all Currency in a wallet
            var currList = await _walletCurRepo.GetCurrenciesListInAWallet(wallId);
            foreach (var curr in currList)
            {
                var fromListCurrId = curr.CurrencyId;
                if (fromListCurrId != mainCurrId)
                {
                    var curConv = await ConvertCurrencyToBaseCurrency(wallId, mainCurrId, fromListCurrId);
                    if (curConv != null)
                    {
                        //Delete the Currency
                        await _walletCurRepo.Delete(curr);
                    }
                }
                break;
            }
            return new Tuple<bool, string>(true, $"Other Currencies Deleted");
        }

        public async Task<List<WalletCurrency>> ListAllCurrencies(string wallId)
        {

            return await _walletCurRepo.GetCurrenciesListInAWallet(wallId);
        }

        public async Task<bool> MergeWallets(string userId)
        {
            var res = false;
            //Get Main wallet
            var mainWallet = await _walletServe.GetMainWallet(userId);
            var mainWalletMainCurrency = await GetMainCurrency(mainWallet.Id);

            //List the wallets in the Account

            var wallets = await _walletServe.GetAllWalletLists();
            for (int i = 0; i < wallets.Count; i++)
            {
                //Each wallet converts the currency list to Main
                var currencyList = await ListAllCurrencies(wallets[i].Id);
                for (int j = 0; i < currencyList.Count; j++)
                {
                    //Remove each Currency After Conversion
                    await RemoveACurrency(currencyList[i].CurrencyId, wallets[i].Id);
                }
            }
            foreach (var wallet in wallets)
            {
                if (mainWallet.Id != wallet.Id)
                {
                    var mainCurrencyList = await ListAllCurrencies(wallet.Id);
                    //Get a list of all the currencies value in the main Wallet main Currency
                    for (int k = 0; k < mainCurrencyList.Count; k++)
                    {
                        var mainCurbal = mainCurrencyList[k].Balance;
                        var mainCurId = mainCurrencyList[k].Id;
                        var convMain = await ConvertCurrencyToCurrency(mainWalletMainCurrency.Id, mainCurrencyList[k].Id);
                        //Add it to the main wallet main currency
                        mainCurrencyList[k].Balance -= mainCurbal;
                        mainWalletMainCurrency.Balance += convMain.Item2;
                        //Update the MainCurrMainWall & Main currency
                        var mainCurrMainWallUpdate = await _walletCurRepo.Edit(mainCurrencyList[k]);
                        var mainCurrUpdate = await _walletCurRepo.Edit(mainWalletMainCurrency);
                        //Delete The currency
                        if (mainCurrMainWallUpdate && mainCurrUpdate)
                        {
                           var delCur= await _walletCurRepo.Delete(mainCurrencyList[k]);


                            //Delete the Wallet
                            var delWall=await _walletServe.DeleteAWallet(wallet.Id);
                            if (delCur && delWall)
                            {
                                res = true;
                            }
                        }
                    }
                }
            }
            return res;
        }
    }
}
