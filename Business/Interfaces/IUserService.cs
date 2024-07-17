using Business.Models;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Business.Interfaces
{
    public interface IUserService
    {
        // User Specific
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIdAsync(string email);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetUserByUsernameAsync(string username);
        public Task<UserInfoModel?> GetUserInfoAsync(string Id);
        public Task<(bool, EmailExists)> UpdateUserInfoAsync(UserInfoModel model, string Id);
        public Task<IdentityResult> UpdateUserAsync(User user);
        public Task<bool> DeleteUserByEmailAsync(HttpContext httpContext, string email);
        public Task<bool> ResetPasswordAsync(string email, string password, string token);
        public Task<string?> GeneratePasswordResetTokenAsync(string email);
        public Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        public Task<bool> IsTwoFactorEnabledAsync(User user);

        // Authorization & Registration
        public Task<(string token, IEnumerable<string> roles, LoginResult result)> SignInUserAsync(LoginModel model, HttpContext httpContext);
        public Task<(string token, IEnumerable<string> roles, LoginResult result)> SignInUserWithTwoFactorAsync(HttpContext httpContext, TwoFactorModel model);
        public Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        public Task SetActiveStatusAsync(string userId, bool value);
        public Task<string?> GenerateEmailConfirmationTokenAsync(string email);
        public Task<IdentityResult> ConfirmRegistrationAsync(string email, string confirmationToken);
        public Task<LockoutResult> LockOutUserAsync(LockoutModel model);
        public Task<(RefreshTokenModel? model, IEnumerable<string> roles)> GetRefreshTokenAsync(HttpContext httpContext, bool rememberMe);
        public Task<string> GenerateTokensAsync(HttpContext httpContext, string email, bool rememberMe);
        public Task RemoveTokensAsync(string refreshToken, HttpContext httpContext);

        // User Role Management
        public Task<IdentityResult> AddUserToRoleAsync(User user, string role);
        public Task<IdentityResult> RemoveUserFromRoleAsync(User user, string role);
        public Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        public Task<IEnumerable<string>> GetUserRolesByEmailAsync(string email);

        // User & Grade
        public Task<int?> GetUserGradeIdAsync(string userId);
    }
}
