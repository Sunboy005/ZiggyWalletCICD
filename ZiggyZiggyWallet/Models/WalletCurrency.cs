namespace ZiggyZiggyWallet.Models
{
    public class WalletCurrency:BaseEntity
    {
        public string WalletId { get; set; }
        public string CurrencyId { get; set; }
        public bool IsMain { get; set; } = false;
        public float Balance { get; set; } = 0.00F;
        public Wallet Wallet { get; set; }
        public Currency Currency { get; set; }
    }
}
