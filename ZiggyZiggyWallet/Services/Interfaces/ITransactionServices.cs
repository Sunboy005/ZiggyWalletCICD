using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.Transactions;
using ZiggyZiggyWallet.DTOs.WalletCurrency;
using ZiggyZiggyWallet.Models;
namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface ITransactionServices
    {

        Task<Tuple<bool, TransactionToAdd>> SendMoney(TransactionToAdd model, string sWId, string sCurr, string rWId, float amount, string description);
        Task<TransactionToAdd> Withdrawal(string sWId, string sCurr, string rWId, float amount, string description, string bankName, string accountNo);
        Task<TransactionToAdd> TopUp(TransactionToAdd model, float amount, string currencyId, string wallId, string toppedBy);
       Task<List<Tranx>> WalletTransactionHistory(string wallId);
    }
}
