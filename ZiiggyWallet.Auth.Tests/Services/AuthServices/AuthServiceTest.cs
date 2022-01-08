using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using Xunit;
using ZiggyZiggyWallet.Data.EFCore;
using ZiggyZiggyWallet.DTOs.Users;
using ZiggyZiggyWallet.Models;
using ZiggyZiggyWallet.Services.Implementations;
using ZiggyZiggyWallet.Services.Interfaces;

namespace ZiiggyWallet.Auth.Tests.Services.AuthenticationServices
{

    public class AuthServiceTest : IDisposable
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly SignInManager<AppUser> _signinMgr;
        private readonly IJWTServices _jwtService;
        private readonly RoleManager<IdentityRole> _roleMgr;

        ZiggyDBContext _dbContext;

        public AuthServiceTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<ZiggyDBContext>();

            builder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ZiggyPremiumDB_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true")
                .UseInternalServiceProvider(serviceProvider);
            _dbContext = new ZiggyDBContext(builder.Options);
            _dbContext.Database.Migrate();

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void SeedTheDatabase()
        {

            _dbContext.Database.EnsureCreated();

            try
            {
                var roles = new string[] { "Admin", "Noob", "Elite" };
                if (!_roleMgr.Roles.Any())
                {
                    foreach (var role in roles)
                    {
                        _roleMgr.CreateAsync(new IdentityRole(role));
                    }
                }

                if (!_dbContext.Currencies.Any())
                {
                    _dbContext.AddRangeAsync(new Currency[]
                   {

                        new Currency()
                        {
                            Name = "Naira",
                            ShortCode = "#",
                            Abbrevation = "ngn"

                        },
                        new Currency()
                        {
                            Name = "Dollar",
                            ShortCode = "$",
                            Abbrevation = "usd"

                        },
                        new Currency()
                        {
                            Name = "Pound",
                            ShortCode = "£",
                            Abbrevation = "gbp"

                        },
                        new Currency()
                        {
                            Name = "Canadian Dollar",
                            ShortCode = "C$",
                            Abbrevation = "cnd"

                        },
                        new Currency()
                        {
                            Name = "Australian Dollar",
                            ShortCode = "Au$",
                            Abbrevation = "aud"

                        }
                   });
                    _dbContext.SaveChanges();

                }

                var data = System.IO.File.ReadAllText("Data/EFCore/SeedData.json");
                var ListOfAppUsers = JsonConvert.DeserializeObject<List<AppUser>>(data);

                if (!_userMgr.Users.Any())
                {
                    var counter = 0;
                    var role = roles[0];
                    foreach (var user in ListOfAppUsers)
                    {
                        user.UserName = user.Email;
                        if (counter == 0)
                        {
                            role = roles[0];
                        }
                        else if (counter % 2 == 0)
                        {
                            role = roles[1];
                        }
                        else
                        {
                            role = roles[2];
                        }


                        //  role = counter < 1 ? roles[0] : roles[1]; // tenary operator
                        string[] currList = (from c in _dbContext.Currencies
                                             select c.Id).ToArray();
                        var res = _userMgr.CreateAsync(user, "P@ssw0rd");
                        if (res.Result.Succeeded)
                            //check if the role is not admin

                            if (role != "Admin")
                            {
                                _dbContext.Wallets.AddAsync(new Wallet
                                {
                                    Name = "UpKeep",
                                    Address = Guid.NewGuid().ToString(),
                                    AppUserId = user.Id,
                                    IsMain = true
                                });
                                _dbContext.WalletCurrency.AddAsync(new WalletCurrency
                                {
                                    Balance = +450,
                                    Wallet = user.Wallets.FirstOrDefault(),
                                    CurrencyId = currList[1],
                                    IsMain = true,
                                });
                                if (role != "Admin" && role != "Noob")
                                {

                                    _dbContext.WalletCurrency.AddAsync(new WalletCurrency
                                    {
                                        Balance = +550,
                                        Wallet = user.Wallets.FirstOrDefault(),
                                        CurrencyId = currList[2],
                                        IsMain = false
                                    });
                                }
                            }
                        _userMgr.AddToRoleAsync(user, role);

                        counter++;
                    }
                }
            }
            catch (DbException)
            {
                //log err
            }


        }
        
        [Test]
        public void Login_WithCorrectPasswordForExistingUserName_ReturnsAccountForCorrectUsername(Login model)
        {
            
            _dbContext.Users.Add(new AppUser { Id = Guid.NewGuid().ToString(), Email = "ade@123.com", UserName = "Ade@123.com", FirstName = "Ade", LastName = "Great", IsActive = true, PhoneNumber="0909876789" });
                //Arrange
                AuthService authService = new AuthService(_userMgr,_signinMgr,_jwtService);

                //Act


                //Assert
            }

        }
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
