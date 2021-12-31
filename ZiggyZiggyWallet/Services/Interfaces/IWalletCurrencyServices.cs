using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface IWalletCurrencyServices
    {
        Task<double> GetWalletBalance(string walletId);
        Task<WalletCurrency> GetMainCurrency(string walletId);
        Task<float> GetCurrencyBalance(string walletId, string currencyId);
    }
}
