using Business.Interfaces;
using Business.Models;
using Data.Data;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services
{
    public class JwtService : IJwtService
    {
        private readonly MathQuestDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly JwtSettingsModel _jwtSettings;

        public JwtService(MathQuestDbContext context, UserManager<User> userManager, IOptions<JwtSettingsModel> jwtSettings)
        {
            _context = context;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        // Generate Access & Refresh Tokens
        public async Task<string> GenerateJwtAccessTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Email),
            };

            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            if (user.TwoFactorEnabled)
            {
                claims.Add(new Claim("TwoFactorEnabled", user.TwoFactorEnabled.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? accessToken)
        {
            if (accessToken == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = key,
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false // we are only validating the token signature, not its expiration
            };

            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return principal;
        }

        public async Task<User?> GetUserFromPrincipalAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return null;
            }
            
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }

        // Refresh Token Helper Methods
        // Add/Update/Remove Refresh Tokens from Database
        public async Task StoreRefreshTokenAsync(string userId, string refreshToken, bool rememberMe)
        {
            var token = _context.RefreshTokens.FirstOrDefault(rt => rt.UserId == userId);

            if (token != null)
            {
                _context.Remove(token);
                await _context.SaveChangesAsync();
            }

            var refreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                Expires = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Created = DateTime.Now
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateRefreshTokenAsync(string userId, string oldRefreshToken, string newRefreshToken, bool rememberMe)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == oldRefreshToken && rt.UserId == userId)
                .ConfigureAwait(false);

            if (tokenEntity != null)
            {
                tokenEntity.Token = newRefreshToken;
                tokenEntity.Expires = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays);
                tokenEntity.Created = DateTime.Now;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task RemoveRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken)
                .ConfigureAwait(false);

            if (tokenEntity != null)
            {
                _context.RefreshTokens.Remove(tokenEntity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        // Validate Refresh Token
        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken)
                .ConfigureAwait(false);

            return tokenEntity != null && tokenEntity.Expires > DateTime.UtcNow;
        }

        // Get User From Refresh Token
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken)
                .ConfigureAwait(false);

            var user = tokenEntity?.User;

            if (user != null)
            {
                return user;
            }

            return null;
        }

        // Set Http only cookie for Access Token & Refresh Token
        public void SetAccessTokenCookie(HttpContext httpContext, string accessToken)
        {
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            };
            httpContext.Response.Cookies.Append("AccessToken", accessToken, accessTokenCookieOptions);
        }

        public void SetRefreshTokenCookie(HttpContext httpContext, string refreshToken, bool rememberMe)
        {
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, refreshTokenCookieOptions);
        }

        public void ClearAuthCookies(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey("AccessToken"))
            {
                httpContext.Response.Cookies.Delete("AccessToken");
            }

            if (httpContext.Request.Cookies.ContainsKey("RefreshToken"))
            {
                httpContext.Response.Cookies.Delete("RefreshToken");
            }
        }
    }
}
