using System.Threading.Tasks;

namespace ApiAuthenticator.Services
{
    public interface IAuthenticationService
    {
        ValueTask<bool> SignInAsync(string login, string password);
        ValueTask<object> GenerateToken();
    }
}
