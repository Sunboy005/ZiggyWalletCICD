using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface IWalletRepository:ICRUDRepository
    {
        public Task<List<Models.WalletToReturn>> GetAllWalletsAsync();
        Task<List<Models.WalletToReturn>> GetWalletsByUserId(string userId);
        Task<Models.WalletToReturn> GetWalletByAddress(string address);
    }
}
