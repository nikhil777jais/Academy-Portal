using AcademyPortalAPI.Models;

namespace AcademyPortalAPI.Services.Token
{
    public interface ITokenService
    {
        Task<string> CreateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}