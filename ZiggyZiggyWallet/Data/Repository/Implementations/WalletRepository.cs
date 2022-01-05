using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.EFCore;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Implementations
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ZiggyDBContext _contex;

        public WalletRepository(ZiggyDBContext contex)
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

        public async Task<List<Wallet>> GetAllWalletsAsync()
        {
            return await _contex.Wallets.ToListAsync();
        }

        public async Task<Wallet> GetWalletByAddress(string address)
        {
            return await _contex.Wallets.Where(x => x.Address == address).FirstOrDefaultAsync();
        }

        public async Task<Wallet> GetWalletById(string id)
        {
            return await _contex.Wallets.Where(x => x.Id == id).FirstAsync();
        }

        public async Task<List<Wallet>> GetWalletsByUserId(string userId)
        {
            return await _contex.Wallets.Where(x => x.AppUserId == userId).ToListAsync();
        }

        public async Task<int> RowCount()
        {
            return await _contex.Wallets.CountAsync(); ;
        }

        public async Task<bool> SaveChanges()
        {
            return await _contex.SaveChangesAsync() > 0;
        }
    }
}
