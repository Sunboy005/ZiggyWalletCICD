using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZiggyZiggyWallet.Models
{
    public class WalletToReturn:BaseEntity
    {
        [Required]
        [MinLength(5, ErrorMessage = "Wallet Name should not be below 5 letters")]
        public string Name { get; set; }


        [Required]
        [MinLength(8, ErrorMessage = "Wallet Address should not be below 16 letters")]
        public string Address { get; set; }

        public bool IsMain { get; set; } = false;

        public AppUser AppUsers { get; set; }
        public string AppUserId { get; set; }
        public List<WalletCurrency> WalletCurrency { get; set; }

        public WalletToReturn()
        {
            WalletCurrency = new List<WalletCurrency>();
        }
    }
}
