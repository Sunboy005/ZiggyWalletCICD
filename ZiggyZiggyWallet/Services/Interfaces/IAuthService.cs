using System.Threading.Tasks;
using ZiggyZiggyWallet.DTOs.Users;

namespace ZiggyZiggyWallet.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginCred> Login(string email, string password, bool rememberMe);
    }
}
