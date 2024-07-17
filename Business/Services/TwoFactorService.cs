using Business.Interfaces;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using System.Text.Encodings.Web;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{
    public class TwoFactorService : ITwoFactorService
    {
        private readonly UserManager<User> _userManager;
        private readonly UrlEncoder _urlEncoder;

        public TwoFactorService(UserManager<User> userManager, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _urlEncoder = urlEncoder;
        }

        public async Task<(string? key, string? image)?> GenerateTwoFactorAsync(string? userId)
        {
            if (userId == null)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return null;
            }

            var key = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
                key = await _userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            }

            var email = _urlEncoder.Encode(user.Email!);
            var appName = _urlEncoder.Encode("MathQuest");
            var qrCodeUrl = GenerateQrCodeUrl(email, appName, key!);

            var qrCodeImage = GenerateQrCodeImage(qrCodeUrl);

            if (qrCodeImage == null)
            {
                return null;
            }

            var contentType = "image/png";
            var fileName = "qr_code.png";
            var fileContentResult = new FileContentResult(qrCodeImage, contentType)
            {
                FileDownloadName = fileName,
            };

            return (key, Convert.ToBase64String(qrCodeImage));
        }

        // This is for setup verification if setup is configured correctly it will enable two factor verification
        public async Task<bool> VerifyTwoFactorTokenAsync(string? userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || token == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, token).ConfigureAwait(false);
            await _userManager.SetTwoFactorEnabledAsync(user, result);

            return result;
        }

        public async Task<bool> DisableTwoFactorAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        #region Helper Methods
        // Generation Helper Methods
        private static string GenerateQrCodeUrl(string email, string appName, string key)
        {
            return $"otpauth://totp/{appName}:{email}?secret={key}&issuer={appName}&digits=6";
        }

        private static byte[] GenerateQrCodeImage(string qrCodeUrl)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrCodeUrl, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (var ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, ImageFormat.Png);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }

        // Remember & Delete TwoFactor Device
        public void SetTwoFactorRememberDeviceCookie(HttpContext httpContext, string userId)
        {
            var CookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMonths(1)
            };
            httpContext.Response.Cookies.Append("remember_device", GenerateRememberDeviceToken(), CookieOptions);
            httpContext.Response.Cookies.Append("twofactor-userid", userId, CookieOptions);

        }

        private static string GenerateRememberDeviceToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public void DeleteTwoFactorRememberDeviceCookie(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("remember_device");
            httpContext.Response.Cookies.Delete("twofactor-userid");
        }

        #endregion
    }
}
