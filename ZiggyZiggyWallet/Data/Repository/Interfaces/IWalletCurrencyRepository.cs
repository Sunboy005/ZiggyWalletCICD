using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface IWalletCurrencyRepository:ICRUDRepository
    {
        Task<WalletCurrency> GetWalletCurrencyDetails(string walletId, string currencyId);
        Task<WalletCurrency> GetMainCurrency(string walletId);
        Task<List<WalletCurrency>> GetCurrenciesListInAWallet(string walletId);
        Task<WalletCurrency> GetWalCurByCurrId(string currId);
    }
}
