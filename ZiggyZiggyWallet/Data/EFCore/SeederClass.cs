using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Data.EFCore
{
    public class SeederClass
    {
        private readonly ZiggyDBContext _ctx;
        private readonly UserManager<AppUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        public SeederClass(ZiggyDBContext ctx,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userMgr = userManager;
            _roleMgr = roleManager;
        }

        public async Task SeedMe()
        {
            _ctx.Database.EnsureCreated();

            try
            {
                var roles = new string[] { "Admin", "Noob", "Elite" };
                if (!_roleMgr.Roles.Any())
                {
                    foreach (var role in roles)
                    {
                        await _roleMgr.CreateAsync(new IdentityRole(role));
                    }
                }

                if (!_ctx.Currencies.Any())
                {
                    await _ctx.AddRangeAsync(new Currency[]
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
                    _ctx.SaveChanges();

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
                            role= roles[0];
                        }
                        else if (counter % 2==0)
                        {
                            role=roles[1];
                        }
                        else
                        {
                            role = roles[2];
                        }


                        //  role = counter < 1 ? roles[0] : roles[1]; // tenary operator
                        string[] currList = (from c in _ctx.Currencies
                                             select c.Id).ToArray();
                        var res = await _userMgr.CreateAsync(user, "P@ssw0rd");
                        if (res.Succeeded)
                            //check if the role is not admin
                           
                        if (role != "Admin")
                            {
                                await _ctx.Wallets.AddAsync(new Wallet
                                {
                                    Name = "UpKeep",
                                    Address = Guid.NewGuid().ToString(),
                                    AppUserId = user.Id,
                                    IsMain = true
                                });
                                await _ctx.WalletCurrency.AddAsync(new WalletCurrency
                                {
                                    Balance =+ 450,                                    
                                    Wallet = user.Wallets.FirstOrDefault(),
                                    CurrencyId = currList[1],
                                    IsMain = true,
                                });
                                if (role != "Admin" && role!="Noob")
                                {
                                    
                                    await _ctx.WalletCurrency.AddAsync(new WalletCurrency
                                    {
                                        Balance =+ 550,
                                        Wallet = user.Wallets.FirstOrDefault(),
                                        CurrencyId = currList[2],
                                        IsMain = false
                                    });
                                }
                            }
                        await _userMgr.AddToRoleAsync(user, role);

                        counter++;
                    }
                }
            }
            catch (DbException)
            {
                //log err
            }


        }
    }
}
