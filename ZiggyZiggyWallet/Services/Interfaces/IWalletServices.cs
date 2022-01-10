using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface IWalletServices
    {
        Task<Tuple<bool, WalletToAdd>> AddAWallet(WalletToAdd model, string userId);
        Task<List<Wallet>> GetAllUsersWalletsList(string userId);
        Task<Wallet> GetMainWallet(string userId);
        Task<Wallet> GetWalletByAddress(string address);
        Task<Wallet> GetWalletbyId(string id);
        Task<List<Wallet>> GetAllWalletLists();
        Task<bool> DeleteAWallet(Wallet wallet);
    }
}
