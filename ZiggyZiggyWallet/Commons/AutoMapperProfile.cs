using AutoMapper;
using ZiggyZiggyWallet.DTOs;
using ZiggyZiggyWallet.DTOs.Users;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Commons
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AppUser, UserToReturn>();
            CreateMap<AppUser, RegisterSuccess>()
                .ForMember(dest => dest.UserId, x => x.MapFrom(x => x.Id))
                .ForMember(d => d.FullName, x => x.MapFrom(x => $"{x.FirstName} {x.LastName}"));

            CreateMap<Register, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(u => u.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(u => u.Phone));
            //.ForMember(dest => dest.Address, x => x.MapFrom(s => new Address { Street = s.Street, State = s.State, Country = s.Country }));

            CreateMap<WalletToAdd, Wallet>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(u => u.Name));

            
            CreateMap<Wallet, WalletToReturn>();

            //CreateMap<Photo, PhotoToReturnDto>();
        }
    }

}
