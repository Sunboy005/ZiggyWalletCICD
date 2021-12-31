using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface IWalletCurrencyRepository:ICRUDRepository
    {
        Task<WalletCurrency> GetWalletCurrency(string walletId, string currencyId);
        Task<WalletCurrency> GetMainCurrency(string walletId);
        Task<List<WalletCurrency>> GetCurrenciesInAWallet(string walletId);
    }
}
