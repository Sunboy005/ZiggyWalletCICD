using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.Currency;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface ICurrencyRepository : ICRUDRepository
    {
        Task<List<CurrencyToReturn>> GetCurrencyList(string address);
    }
}
