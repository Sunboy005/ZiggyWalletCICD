using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;
using WalletToReturn = ZiggyZiggyWallet.DTOs.WalletToReturn;

namespace ZiggyZiggyWallet.Data.Repository.Interfaces
{
    public interface IWalletRepository : ICRUDRepository
    {
        Task<List<Wallet>> GetAllWalletsAsync();
        Task<List<Wallet>> GetWalletsByUserId(string userId);
        Task<Wallet> GetWalletByAddress(string address);
        Task<Wallet> GetWalletById(string id);
    }
}
