namespace ZiggyZiggyWallet.DTOs
{
    public class WalletCurrencyToReturn
    {
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public float Balance { get; set; }
        public bool IsMain { get; set; }
    }
}
