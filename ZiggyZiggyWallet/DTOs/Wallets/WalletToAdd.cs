namespace ZiggyZiggyWallet.DTOs
{
    public class WalletToAdd
    {
        public string Address { get; set; }
        public bool IsMain { get; set; } = false;
        public string Name { get; set; }

    }
}
