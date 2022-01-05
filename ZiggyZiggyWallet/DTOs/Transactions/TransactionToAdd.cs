using System.ComponentModel.DataAnnotations;

namespace ZiggyZiggyWallet.DTOs.Transactions
{
    public class TransactionToAdd
    {
        public float AmountSent { get; set; }
        public float AmountReceived { get; set; }

        public string Description { get; set; }


        public string SenderCurrency { get; set; }
        public string RecieverCurrency { get; set; }


        [Required]
        [MinLength(8, ErrorMessage = "Senders Wallet should not be below 8 letters")]
        public string SenderWalletId { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Reciepient Wallet should not be below 8 letters")]
        public string RecipientWalletId { get; set; }

        public string Status { get; set; }

        public string TranxType { get; set; }

    }
}
