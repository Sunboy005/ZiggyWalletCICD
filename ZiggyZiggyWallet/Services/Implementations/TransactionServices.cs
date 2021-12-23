using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class TransactionServices:ITransactionServices
    {
        public Task<Transaction> SendMoney(string currency, string walletAdderess, float amount)
        {
            throw new System.NotImplementedException();
        }

        public Task<Transaction> TopUp(float amount, string currency, string walletAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task<Transaction> Withdrawal(float amount, string bank, string accountNo)
        {
            throw new System.NotImplementedException();
        }
    }
}
