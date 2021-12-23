using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZiggyZiggyWallet.Models
{
    public class AppUser:IdentityUser
    {

            [Required]
            [StringLength(15, MinimumLength = 3, ErrorMessage = "Must be between 3 and 15")]
            public string LastName { get; set; }

            [Required]
            [StringLength(15, MinimumLength = 3, ErrorMessage = "Must be between 3 and 15")]
            public string FirstName { get; set; }

            public bool IsActive { get; set; }

            public byte[] PasswordSalt { get; set; }

            public List<WalletToReturn> Wallets { get; set; }//1 to many

        // Noob User ===> Only one wallet, and one currency
        // Elite User ===> Multiple wallets, and each of this wallet, can have multiple currencies
        public AppUser()
            {
                Wallets = new List<WalletToReturn>();
            }
        }
}
