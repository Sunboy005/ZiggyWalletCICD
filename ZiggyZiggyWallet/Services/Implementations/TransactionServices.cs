using System.Threading.Tasks;
using System.Transactions;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class TransactionServices:ITransactionServices
    {
        public Task<Tranx> SendMoney(string currency, string walletAdderess, float amount)
        {
            throw new System.NotImplementedException();
            //Get Balance of the Currency

            //If the currency Bal <= Amount to currency send
            //Notify the Wallet User
            //Get the list of user Wallet
            //Select another wallet

            //Convert to currency to Send

            //Add money to currency to send minus it from default currency
            
            //Send Money //Receive Money

            //Check Wallet  to recieve if it has currency

            //If not

            //Check UserRole, If Nobe

            //Convert to Wallet currency

            //else  Add New Currency value is the amount send


        }

        public Task<Tranx> TopUp(float amount, string currency, string walletAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task<Tranx> Withdrawal(float amount, string bank, string accountNo)
        {
            throw new System.NotImplementedException();
        }
    }
}
