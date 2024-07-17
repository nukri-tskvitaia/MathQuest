using Business.Interfaces;
namespace MathQuestWebApi.Services
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            var accessToken = context.Request.Cookies["AccessToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                await ProcessAccessTokenAsync(context, accessToken).ConfigureAwait(true);
            }
            else if (!string.IsNullOrEmpty(refreshToken))
            {
                await ProcessRefreshTokenAsync(context).ConfigureAwait(true);
            }
            else
            {
                context.Response.Redirect("/api/authorization/login");
                return;
            }

            await _next(context).ConfigureAwait(true);
        }

        private static Task ProcessAccessTokenAsync(HttpContext context, string accessToken)
        {
            var _jwtService = context.RequestServices.GetRequiredService<IJwtService>();

            try
            {
                var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
                if (principal != null)
                {
                    context.User = principal;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error processing access token: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private static async Task ProcessRefreshTokenAsync(HttpContext context)
        {
            var _jwtService = context.RequestServices.GetRequiredService<IJwtService>();
            var _userService = context.RequestServices.GetRequiredService<IUserService>();

            try
            {
                var (tokenModel, roles) = await _userService.GetRefreshTokenAsync(context, true).ConfigureAwait(true);
                
                if (tokenModel != null)
                {
                    var newPrincipal = _jwtService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
                    if (newPrincipal != null)
                    {
                        context.User = newPrincipal;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error processing refresh token: {ex.Message}");
            }
        }
    }
}