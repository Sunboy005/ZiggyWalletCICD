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
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ZiggyDBContext _contex;

        public CurrencyRepository(ZiggyDBContext contex)
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

        public Task<List<CurrencyToReturn>> GetCurrencyList(string address)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> RowCount()
        {
            return await _contex.Currencies.CountAsync(); ;
        }

        public async Task<bool> SaveChanges()
        {
            return await _contex.SaveChangesAsync() > 0;
        }
    }
}
