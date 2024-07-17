using Microsoft.AspNetCore.Http;

namespace Business.Interfaces
{
    public interface ITwoFactorService
    {
        Task<(string? key, string? image)?> GenerateTwoFactorAsync(string? userId);
        Task<bool> VerifyTwoFactorTokenAsync(string? userId, string token);
        public Task<bool> DisableTwoFactorAsync(string userId);
        public void SetTwoFactorRememberDeviceCookie(HttpContext httpContext, string userId);
        public void DeleteTwoFactorRememberDeviceCookie(HttpContext httpContext);
    }
}
