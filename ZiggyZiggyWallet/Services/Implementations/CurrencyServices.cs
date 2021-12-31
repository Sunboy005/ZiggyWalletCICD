using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Data.Repository.Interfaces;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiggyZiggyWallet.Services.Implementations
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly ICurrencyRepository _curRepo;
        private readonly IMapper _mapper;

        public CurrencyServices(ICurrencyRepository curRepo,IMapper mapper)
        {
            _curRepo = curRepo;
            _mapper = mapper;
        }
        public async Task<Tuple<bool, CurrencyToAdd>> AddCurrency(CurrencyToAdd model)
        {
            try
            {
                var currency = _mapper.Map<Currency>(model);
                var res = await _curRepo.Add(currency);
                if (res)
                {
                    return new Tuple<bool, CurrencyToAdd>(res, model);
                }
            }
            catch (DbException ex)
            {
                Console.WriteLine(ex.Message);
                //Log Error
            }
            return null;
        }

        public async Task<List<Currency>> GetAllCurrency()
        {
           return await _curRepo.GetCurrencyList();
        }

        public Task<Wallet> GetCurrencyById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
