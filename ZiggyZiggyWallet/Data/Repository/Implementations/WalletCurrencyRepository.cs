using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.EFCore;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Implementations
{
    public class WalletCurrencyRepository : IWalletCurrencyRepository
    {
        private readonly ZiggyDBContext _contex;

        public WalletCurrencyRepository(ZiggyDBContext contex)
        {
            _contex = contex;
        }

        public async Task<bool> Add<T>(T entity)
        {
            _contex.Add(entity);
            return await SaveChanges();
        }

        public async Task<bool> Delete<T>(T entity)
        {
            _contex.Remove(entity);
            return await SaveChanges();
        }

        public async Task<bool> Edit<T>(T entity)
        {
            _contex.Update(entity);
            return await SaveChanges();
        }

        public async Task<WalletCurrency> GetWalletCurrency(string walletId, string currencyId)
        {
            var result = await _contex.WalletCurrency.Include(x => x.Balance).FirstOrDefaultAsync(x => x.WalletId == walletId && x.CurrencyId == currencyId);
            return result;
        }
         public async Task<WalletCurrency> GetMainCurrency(string walletId)
        {
            var result = await _contex.WalletCurrency.Include(x => x.IsMain).FirstOrDefaultAsync(x => x.WalletId == walletId && x.IsMain==true);
            return result;
        }

        public async Task<List<WalletCurrency>> GetCurrenciesInAWallet(string walletId)
        {
           var currencyList= await _contex.WalletCurrency.Where(x => x.WalletId == walletId).ToListAsync();
        return currencyList;
        }


        public async Task<int> RowCount()
        {
            return await _contex.WalletCurrency.CountAsync(); ;
        }

        public async Task<bool> SaveChanges()
        {
            return await _contex.SaveChangesAsync() > 0;
        }
    }
}
