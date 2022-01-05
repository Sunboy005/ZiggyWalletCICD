using System;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.WalletCurrency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface IWalletCurrencyServices
    {
        Task<Tuple<bool, WalletCurrencyToAdd>> AddACurrency(WalletCurrencyToAdd model, string walletId);
        Task<Tuple<bool, float>> ConvertCurrencyToBaseCurrency(string walletId, string baseCurrencyId, string fromCurrencyId);
        Task<Tuple<bool, float>> ConvertCurrencyToCurrency(string toCurrencyId, string fromCurrencyId);
        Task<double> GetWalletBalance(string walletId);
        Task<WalletCurrency> GetMainCurrency(string walletId);
        Task<float> GetCurrencyBalance(string walletId, string currencyId);
        Task<bool> CheckCurrencyInWallet(string walletAddress, string currencyId);
    }
}
