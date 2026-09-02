using Entites.Model;

namespace HRMSAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
