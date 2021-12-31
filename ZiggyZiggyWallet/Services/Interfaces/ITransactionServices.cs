using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;
namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface ITransactionServices
    {

        Task<Tranx> SendMoney(string currency, string walletAdderess, float amount);
        Task<Tranx> Withdrawal(float amount, string bank, string accountNo);
        Task<Tranx> TopUp(float amount, string currency, string walletAddress);
    }
}
