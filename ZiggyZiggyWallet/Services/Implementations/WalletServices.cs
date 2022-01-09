using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class WalletServices : IWalletServices
    {
        private readonly IWalletRepository _walletRepo;
        private readonly UserManager<AppUser> _userMng;

        public WalletServices(IWalletRepository walletRepo, UserManager<AppUser> userMng)
        {
            _walletRepo = walletRepo;
            _userMng = userMng;

        }
        public async Task<Tuple<bool, WalletToAdd>> AddAWallet(WalletToAdd model, string userId)
        {

            var user = await _userMng.Users.Include(x => x.Wallets).FirstOrDefaultAsync(x => x.Id == userId);

            var wallet = AutoMapper.Mapper.Map<Wallet>(model);
            wallet.AppUserId = userId;

            if (!user.Wallets.Any(x => x.IsMain))
                wallet.IsMain = true;

            var res = await _walletRepo.Add(wallet);

            return new Tuple<bool, WalletToAdd>(res, model);
        }


        public async Task<List<Wallet>> GetAllUsersWalletsList(string userId)
        {
            return await _walletRepo.GetWalletsByUserId(userId);
        }

        public async Task<List<Wallet>> GetAllWalletLists()
        {
            return await _walletRepo.GetAllWalletsAsync();
        }

        public async Task<Wallet> GetWalletByAddress(string address)
        {
            var res = await _walletRepo.GetWalletByAddress(address);
            return res;
        }

        public async Task<Wallet> GetWalletbyId(string id)
        {
            var res = await _walletRepo.GetWalletById(id);
            return res;
        }

        public async Task<Wallet> GetMainWallet(string userId)
        {
            var res = await _walletRepo.GetWalletsByUserId(userId);

            var mainWallet = res.FirstOrDefault(x => x.IsMain == true);

            if (mainWallet != null)
                return mainWallet;

            return null;
        }
        public async Task<bool> DeleteAWallet(string walletId)
        {
            return await _walletRepo.Delete(walletId);
        }
    }
}
