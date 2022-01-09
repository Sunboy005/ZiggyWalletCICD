using System;

namespace ZiggyZiggyWallet.Models
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; } = DateTime.Now.ToString();
    }
}
