using AcademyPortal.Models;

namespace BingoAPI.Services.Token
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}