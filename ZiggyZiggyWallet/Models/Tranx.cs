using System.ComponentModel.DataAnnotations;

namespace ZiggyZiggyWallet.Models
{
    public class Tranx:BaseEntity
    {
        [Required]
        public float Amount { get; set; } = 0.00F;

        public string Description { get; set; }


        public string Currency { get; set; }


        [Required]
        [MinLength(16, ErrorMessage = "SenderName should not be below 16 letters")]
        public string SenderWalletId { get; set; }

        [Required]
        [MinLength(16, ErrorMessage = "Reciepient Wallet should not be below 6 letters")]
        public string RecipientWalletId { get; set; }

        public string WalletsId { get; set; }

        public string WalletId { get; set; }

        public string StatusId { get; set; }

        //public Status Status { get; set; }
        public string TranxTypeId { get; set; }
        //public TranxType TranxType { get; set; }
    }
}
