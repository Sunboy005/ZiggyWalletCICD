using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface ICurrencyRepository:ICRUDRepository
    {
        Task<List<Currency>> GetCurrencyInAWallet(string address);
        Task<List<Currency>> GetCurrencyBalance(string address);
    }
}
