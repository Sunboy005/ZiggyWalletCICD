namespace ZiggyZiggyWallet.DTOs.WalletCurrency
{
    public class WalletCurrencyToAdd
    {
        public string CurrencyId { get; set; }
        public float Balance { get; set; } = 0.00F;
        public bool IsMain { get; set; } = false;
        public string WalletId { get; set; }
    }
}
