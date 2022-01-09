using AutoMapper;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.DTOs.Transactions;
using ZiggyZiggyWallet.DTOs.Users;
using ZiggyZiggyWallet.DTOs.WalletCurrency;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Commons
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //UserMapping
            CreateMap<AppUser, UserToReturn>();
            CreateMap<AppUser, RegisterSuccess>()
                .ForMember(dest => dest.UserId, x => x.MapFrom(x => x.Id))
                .ForMember(d => d.FullName, x => x.MapFrom(x => $"{x.FirstName} {x.LastName}"));

            CreateMap<Register, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(u => u.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(u => u.Phone));
            //.ForMember(dest => dest.Address, x => x.MapFrom(s => new Address { Street = s.Street, State = s.State, Country = s.Country }));

            //Wallets Mapping
            CreateMap<WalletToAdd, Wallet>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(u => u.Name));

            CreateMap<Wallet, WalletToReturn>();

            //Currencies Mapping
            CreateMap<CurrencyToAdd, Currency>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(u => u.Name))
                .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(u => u.ShortCode))
                .ForMember(dest => dest.Abbrevation, opt => opt.MapFrom(u => u.Abbrevation));

            CreateMap<Currency, CurrencyToReturn>();

            //Transactions Mapping
            CreateMap<TransactionToAdd, Tranx>()
               .ForMember(dest => dest.TranxType, opt => opt.MapFrom(u => u.TranxType))
               .ForMember(dest => dest.RecipientWalletId, opt => opt.MapFrom(u => u.RecipientWalletId))
               .ForMember(dest => dest.AmountReceived, opt => opt.MapFrom(u => u.AmountReceived))
               .ForMember(dest => dest.AmountSent, opt => opt.MapFrom(u => u.AmountSent))
               .ForMember(dest => dest.SenderWalletId, opt => opt.MapFrom(u => u.SenderWalletId))
               .ForMember(dest => dest.RecieverCurrency, opt => opt.MapFrom(u => u.RecieverCurrency))
               .ForMember(dest => dest.SenderCurrency, opt => opt.MapFrom(u => u.SenderCurrency))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(u => u.Description));

            CreateMap<Tranx, TransactionToReturn>();

            //WalletCurrency Mapping
            CreateMap<WalletCurrencyToAdd, WalletCurrency>()
               .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(u => u.CurrencyId))
               .ForMember(dest => dest.Balance, opt => opt.MapFrom(u => u.Balance))
               .ForMember(dest => dest.IsMain, opt => opt.MapFrom(u => u.IsMain))
               .ForMember(dest => dest.WalletId, opt => opt.MapFrom(u => u.WalletId));
               
            CreateMap<WalletCurrency, WalletCurrencyToReturn>();
                

        }
    }

}
