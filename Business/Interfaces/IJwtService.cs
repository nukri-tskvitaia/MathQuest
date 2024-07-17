using Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Business.Interfaces
{
    public interface IJwtService
    {
        // Generate Tokens
        public Task<string> GenerateJwtAccessTokenAsync(User user);
        public string GenerateRefreshToken();

        //Access Token Helper Methods
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? accessToken);
        public Task<User?> GetUserFromPrincipalAsync(ClaimsPrincipal principal);

        // Refresh Token Helper Methods
        public Task StoreRefreshTokenAsync(string userId, string refreshToken, bool rememberMe);
        public Task RemoveRefreshTokenAsync(string refreshToken);
        public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        public Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        public Task UpdateRefreshTokenAsync(string userId, string oldRefreshToken, string newRefreshToken, bool rememberMe);
        
        // Token Cookies Helper Methods
        public void SetAccessTokenCookie(HttpContext httpContext, string accessToken);
        public void SetRefreshTokenCookie(HttpContext httpContext, string refreshToken, bool rememberMe);
        public void ClearAuthCookies(HttpContext httpContext);
    }
}
