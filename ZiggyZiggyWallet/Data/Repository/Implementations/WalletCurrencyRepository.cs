using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.EFCore;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Currency;

namespace ZiggyZiggyWallet.Data.Repository.Implementations
{
    public class WalletCurrencyRepository:IWalletCurrencyRepository
    {
        private readonly ZiggyDBContext _contex;

        public WalletCurrencyRepository(ZiggyDBContext contex)
        {
            _contex = contex;
        }
        public async Task<WalletCurrencyToReturn> GetCurrencyBalance(string address, string currency)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<CurrencyToReturn>> GetCurrencyInAWallet(string walletId)
        {
            throw new System.NotImplementedException();
        }

    }
}
