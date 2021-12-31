using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface ICurrencyRepository : ICRUDRepository
    {
        Task<Currency> GetCurrencyfromId(string id);

        Task<List<Currency>> GetCurrencyList();
    }
}
