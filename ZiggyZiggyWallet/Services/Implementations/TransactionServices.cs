using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs.Transactions;
using ZiggyZiggyWallet.DTOs.WalletCurrency;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IWalletCurrencyRepository _walletCurRepo;
        private readonly ICurrencyRepository _curRepo;
        private readonly IWalletRepository _walletRepo;
        private readonly IWalletCurrencyServices _walletCurServe;
        private readonly ITransactionsRepository _tranxRepo;
        private readonly UserManager<AppUser> _userMgr;
        private readonly IMapper _mapper;

        public TransactionServices(ITransactionsRepository tranxRepo, IWalletCurrencyRepository walletCurRepo, UserManager<AppUser> userMgr, IMapper mapper, IWalletCurrencyServices walletCurServe, IWalletRepository walletRepo, ICurrencyRepository curRepo)
        {
            _walletCurRepo = walletCurRepo;
            _curRepo = curRepo;
            _userMgr = userMgr;
            _mapper = mapper;
            _walletRepo = walletRepo;
            _walletCurServe = walletCurServe;
            _tranxRepo = tranxRepo;

        }
        public async Task<Tuple<bool, TransactionToAdd>> SendMoney(TransactionToAdd model, string sWId, string sCurr, string rWId, float amount, string description)
        {
            //Get SenderWallet Currency Details (sCurrDet) 
            var sWallCurrDet = await _walletCurRepo.GetWalletCurrencyDetails(sWId, sCurr);
            var sWallCurrBal = sWallCurrDet.Balance;

            //Get Sender Currency Details (sCurrDet)
            var sCurrDet = await _curRepo.GetCurrencyfromId(sCurr);
            var sCurrName = sCurrDet.Name;
            var sCurrAbb = sCurrDet.Abbrevation;
            var sCurrShortCode = sCurrDet.ShortCode;

            if (sWallCurrBal < amount)
            {
                //Get Currencies List in the Wallet
                var otherWallCurrList = _walletCurRepo.GetCurrenciesListInAWallet(sWId).Result;
                var noOfcurr = otherWallCurrList.Count;
                //If count of WalletCurrList is more than one
                if (noOfcurr >= 2)
                {
                    for (int i = 0; i < noOfcurr; i++)
                    {
                        //if Sender Currency Balance is less
                        while (sWallCurrBal < amount)
                        {
                            //Get Currency Details
                            var newCurrDet = await _walletCurRepo.GetWalletCurrencyDetails(sWId, otherWallCurrList[i].CurrencyId);
                            if (newCurrDet == null)
                            {
                                return null;
                            }
                            var newCurrBal = newCurrDet.Balance;
                            var newCurrId = newCurrDet.CurrencyId;
                            //Check if it is not sender Currency 
                            if (newCurrId != sCurrDet.Id)
                            {
                                //Perform Conversion
                                var curConv = _walletCurServe.ConvertCurrencyToCurrency(newCurrId, sCurrDet.Id);

                                newCurrDet.Balance -= newCurrBal;
                                if (newCurrBal == 0)
                                    break;
                                sWallCurrDet.Balance += (curConv.Result.Item2 * newCurrBal);
                                await _walletCurRepo.Edit(sCurrDet);
                                await _walletCurRepo.Edit(newCurrDet);
                            }
                            break;
                        }
                        //continue;6
                    }
                }

                if (sWallCurrBal < amount)
                    return null;

            }
            //Perform The Transfer
            //Check the reciever's Wallet if currency Exist
            var rCurr = await _walletCurServe.CheckCurrencyInWallet(rWId, sCurrDet.Id);
            var amtToAdd = 0.00F;
            //Get Wallet Currency Details and Role of the Reciever
            var rWalletDetails = await _walletRepo.GetWalletById(rWId);
            var rWalletCurrDetails = await _walletCurServe.GetMainCurrency(rWId);
            var rCurrId = rWalletCurrDetails.CurrencyId;
            var rCurrDet = await _curRepo.GetCurrencyfromId(rCurrId);
            var rCurrName = rCurrDet.Name;
            //Initiate The actions
            var first = false;
            var second = false;

            //Get Wallet Details           
            var rWallCurrDet = _walletCurRepo.GetWalletCurrencyDetails(rWId, sCurr).Result;
            var rWallCurrBal = 0.00F;


            if (rCurr == false)
            {
                var recieversId = rWalletDetails.AppUserId;
                var reciever = await _userMgr.FindByIdAsync(recieversId);
                var recRole = await _userMgr.GetRolesAsync(reciever);

                //Check the User Role
                if (recRole.Contains("Noob"))
                {
                    //Get Main Currency
                    rWalletCurrDetails = await _walletCurServe.GetMainCurrency(rWId);
                    rCurrId = rWalletCurrDetails.CurrencyId;
                    rCurrDet = await _curRepo.GetCurrencyfromId(rCurrId);
                    rCurrName = rCurrDet.Name;
                    //Convert amount in SCurr to amount in Noob's Main Currency
                    var sCurrToNoob = await _walletCurServe.ConvertCurrencyToCurrency(rCurrId, sCurr);

                    //amount To Add
                    amtToAdd = sCurrToNoob.Item2;

                    //Perform the Transaction
                    sWallCurrDet.Balance -= amount;
                    rWalletCurrDetails.Balance += amtToAdd;

                    //Update the database
                    first = await _walletCurRepo.Edit(sCurrDet);
                    second = await _walletCurRepo.Edit(rWalletCurrDetails);

                }
                else
                {
                    var model2 = new WalletCurrencyToAdd();
                    var walletToCreate = _mapper.Map<WalletCurrency>(model2);
                    walletToCreate.CurrencyId = sCurr;
                    walletToCreate.Balance = model2.Balance;
                    walletToCreate.IsMain = model2.IsMain;
                    walletToCreate.WalletId = rWId;


                    var createCurr = await _walletCurRepo.Add(walletToCreate);
                    if (createCurr != null)
                    {
                        //Cet Newly Created WalletCurrency Details
                        var rWallCurrDetails = await _walletCurRepo.GetWalletCurrencyDetails(rWId, sCurr);

                        //amount To Add
                        amtToAdd = amount;

                        //Perform the Transaction
                        sWallCurrDet.Balance -= amount;
                        rWallCurrDetails.Balance += amtToAdd;

                        //Update the database
                        first = await _walletCurRepo.Edit(sCurrDet);
                        second = await _walletCurRepo.Edit(rWallCurrDetails);
                    }
                }
            }
            else
            {

                //amount To Add
                amtToAdd = amount;

                //Perform the Transaction
                sWallCurrDet.Balance -= amount;
                rWallCurrDet.Balance += amtToAdd;

                //Update the database
                first = await _walletCurRepo.Edit(sCurrDet);
                second = await _walletCurRepo.Edit(rWallCurrDet);
            }

            //rCurrName=rWalletCurrDetails.Balance
            //var recCurr = rWallCurrDet.Currency;
            var tranxToAdd = _mapper.Map<Tranx>(model);
            tranxToAdd.AmountSent = amount;
            tranxToAdd.AmountReceived = amtToAdd;
            tranxToAdd.Description = description;
            tranxToAdd.SenderCurrency = sCurrDet.Name;
            tranxToAdd.RecieverCurrency = rCurrName;
            tranxToAdd.RecipientWalletId = rWId;
            tranxToAdd.SenderWalletId = sWId;
            //Status of the Account
            if (first && second)
            {
                tranxToAdd.Status = "Successful";
            }
            if ((first && !second) || (!first && second))
            {
                sWallCurrDet.Balance = sWallCurrBal;
                rWallCurrDet.Balance = rWallCurrBal;
                await _walletCurRepo.Edit(sCurrDet);
                await _walletCurRepo.Edit(rWallCurrDet);
                tranxToAdd.Status = "Reversed";
            }
            if ((!first && !second))
            {

                tranxToAdd.Status = "Failed";
            }
            tranxToAdd.TranxType = "Transfer";


            var res = await _tranxRepo.Add(tranxToAdd);

            return new Tuple<bool, TransactionToAdd>(res, model);
        }

        public async Task<TransactionToAdd> TopUp(TransactionToAdd model, float amount, string currencyId, string wallId, string toppedBy)
        {
            //Get The walletDetails
            var wallCurDetails = await _walletCurRepo.GetWalletCurrencyDetails(wallId, currencyId);
            var CurrDet = await _curRepo.GetCurrencyfromId(currencyId);
            var CurrName = CurrDet.Name;

            if (wallCurDetails == null)
            {
                return null;
            }
            wallCurDetails.Balance = amount;
            var first = await _walletCurRepo.Edit(wallCurDetails);

            var tranxToAdd = _mapper.Map<Tranx>(model);
            tranxToAdd.AmountSent = amount;
            tranxToAdd.AmountReceived = amount;
            tranxToAdd.Description = "TopUp From Admin";
            tranxToAdd.SenderCurrency = CurrName;
            tranxToAdd.RecieverCurrency = CurrName;
            tranxToAdd.RecipientWalletId = wallId;
            tranxToAdd.SenderWalletId = toppedBy;
            //Status of the Account
            if (first)
            {
                tranxToAdd.Status = "Successful";
            }
            if ((!first))
            {

                tranxToAdd.Status = "Failed";
            }
            tranxToAdd.TranxType = "TopUp";

            var res = await _tranxRepo.Add(tranxToAdd);

            return model;
        }

        //public async Task<Tuple<bool, TransactionToAdd>> AddATransaction(TransactionToAdd model)
        //{
        //    var tranxMapped = _mapper.Map<Tranx>(model);
        //    tranxMapped.TranxType = model.TranxType;
        //    tranxMapped.AmountReceived = model.AmountReceived;
        //    tranxMapped.AmountSent = model.AmountSent;
        //    tranxMapped.Description = model.Description;
        //    tranxMapped.SenderWalletId = model.SenderWalletId;
        //    tranxMapped.RecipientWalletId = model.RecipientWalletId;
        //    tranxMapped.Status = model.Status;
        //    tranxMapped.RecieverCurrency = model.RecieverCurrency;
        //    tranxMapped.SenderCurrency = model.SenderCurrency;


        //    var res = await _tranxRepo.Add(tranxMapped);

        //    return new Tuple<bool, TransactionToAdd>(res, model);
        //}
    public Task<TransactionToAdd> Withdrawal(string sWId, string sCurr, string rWId, float amount, string description, string bankName, string accountNo)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Tranx>> WalletTransactionHistory(string wallId)
    {
        return await _tranxRepo.GetTransactionsByWallet(wallId);

    }
}
}
