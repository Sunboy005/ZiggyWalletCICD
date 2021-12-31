using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface ICurrencyServices
    {
        Task<Tuple<bool, CurrencyToAdd>> AddCurrency(CurrencyToAdd model);
        Task<List<Currency>> GetAllCurrency();
        Task<Currency> GetCurrencyById(string id);
    }
}
