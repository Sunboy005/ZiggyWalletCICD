using AutoMapper;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Currency;
using ZiggyZiggyWallet.DTOs.Users;
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

        }
    }

}
