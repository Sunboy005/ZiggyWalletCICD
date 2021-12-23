using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZiggyZiggyWallet.Models
{
    public class Currency:BaseEntity
    {
        public Currency()
        {
            WalletCurrency = new List<WalletCurrency>();

        }
        [Required]
        [MinLength(3, ErrorMessage = "Currency Name should be more than 3 letters")]
        public string Name { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "ShortCode should not be below 2 letters")]
        public string ShortCode { get; set; }


        public List<WalletCurrency> WalletCurrency { get; set; }
    }
}
