using System;

namespace ZiggyZiggyWallet.DTOs
{
    public class WalletToAdd
    {
        public string Name { get; set; }
        public string Address { get; set; }= Guid.NewGuid().ToString();
        public bool IsMain { get; set; }= false;

    }
}
