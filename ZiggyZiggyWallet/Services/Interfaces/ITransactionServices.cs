using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;
namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface ITransactionServices
    {

        Task<Transaction> SendMoney(string currency, string walletAdderess, float amount);
        Task<Transaction> Withdrawal(float amount, string bank, string accountNo);
        Task<Transaction> TopUp(float amount, string currency, string walletAddress);
    }
}
