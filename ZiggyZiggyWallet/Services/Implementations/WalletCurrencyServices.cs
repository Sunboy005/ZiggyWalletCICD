using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class WalletCurrencyServices : IWalletCurrencyServices
    {
        private readonly IWalletCurrencyRepository _walletCurRepo;
        private readonly ICurrencyRepository _curRepo;
        private readonly UserManager<AppUser> _userMng;
        private readonly Mapper _mapper;

        public WalletCurrencyServices(IWalletCurrencyRepository walletCurRepo, ICurrencyRepository curRepo, UserManager<AppUser> userMng, Mapper mapper)
        {
            _walletCurRepo = walletCurRepo;
            _userMng = userMng;
            _mapper = mapper;
            _curRepo = curRepo;

        }
        public async Task<float> GetCurrencyBalance(string walletAddress, string currencyId)
        {
            var res = await _walletCurRepo.GetWalletCurrency(walletAddress, currencyId);
            return res.Balance;
        }

        public async Task<WalletCurrency> GetMainCurrency(string walletId)
        {
            var res = await _walletCurRepo.GetMainCurrency(walletId);

            //var currency = await _curRepo.GetCurrencyfromId(res);
            return res;
        }

        public async Task<double> GetWalletBalance(string walletid)
        {
            var totalBal = 0.00;
            var res = await _walletCurRepo.GetCurrenciesInAWallet(walletid);

            foreach (var item in res)
            {
                var main = _walletCurRepo.GetMainCurrency(walletid);
                //Convert each currency to main
                totalBal += item.Balance;


            }
            return  totalBal;
        }
    }
}
