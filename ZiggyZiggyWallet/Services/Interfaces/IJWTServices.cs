using System.Collections.Generic;
using ZiggyZiggyWallet.Models;

namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface IJWTServices
    {
        public string GenerateToken(AppUser user, List<string> userRoles);
    }
}
 