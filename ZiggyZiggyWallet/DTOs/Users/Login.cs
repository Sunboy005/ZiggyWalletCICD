using System.ComponentModel.DataAnnotations;

namespace ZiggyZiggyWallet.DTOs.Users
{
    public class Login
    {
        [Required]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
