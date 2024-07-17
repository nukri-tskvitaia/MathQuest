using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Entities.Identity;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ITwoFactorService _twoFactorService;

        public UserService(IUnitOfWork context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IJwtService jwtService, ITwoFactorService twoFactorService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _twoFactorService = twoFactorService;
        }

        // User CRUD Operations
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.Include(u => u.Person).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string email)
        {
            return await _userManager.FindByIdAsync(email);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<UserInfoModel?> GetUserInfoAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            
            if (user == null)
            {
                return null;
            }

            var person = await _context.PersonRepository.GetByIdAsync(user.PersonId);

            var userInfo = _mapper.Map<UserInfoModel>(user);
            userInfo = _mapper.Map(person, userInfo);

            return userInfo;
        }

        public async Task<(bool, EmailExists)> UpdateUserInfoAsync(UserInfoModel model, string Id)
        {
            var emailUser = await _userManager.FindByEmailAsync(model.Email);
            if (emailUser != null && emailUser.Id != Id)
            {
                return (false, EmailExists.Exists);
            }

            var user = emailUser ?? await _userManager.FindByIdAsync(Id).ConfigureAwait(false);
            if (user == null)
            {
                return (false, EmailExists.None);
            }

            await _context.BeginTransactionAsync();
            try
            {
                if (model.Email != user.Email)
                {
                    user.Email = model.Email;

                    var updateUserResult = await _userManager.UpdateAsync(user);

                    if (!updateUserResult.Succeeded)
                    {
                        await _context.RollbackTransactionAsync();
                        return (false, EmailExists.None);
                    }
                }

                var person = _mapper.Map<Person>(model);
                person.Id = user.PersonId;
                _context.PersonRepository.Update(person);
                await _context.SaveAsync();

                await _context.CommitTransactionAsync();
                return (true, EmailExists.None);
            }
            catch (Exception)
            {
                await _context.RollbackTransactionAsync();
                return (false, EmailExists.None);
            }
        }

        public async Task<int?> GetUserGradeIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if ( user == null)
            {
                return null;
            }

            return user.GradeId;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserByEmailAsync(HttpContext httpContext, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var person = await _context.PersonRepository.GetByIdAsync(user.PersonId);

            if (person == null)
            {
                return false;
            }

            var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.UserId == user.Id);

            if (refreshToken == null)
            {
                return false;
            }

            await _jwtService.RemoveRefreshTokenAsync(refreshToken.Token);
            _jwtService.ClearAuthCookies(httpContext);

            await _userManager.DeleteAsync(user);
            _context.PersonRepository.Delete(person);
            await _context.SaveAsync();

            return true;
        }

        // Role management operations
        public async Task<IdentityResult> AddUserToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(User user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<string>> GetUserRolesByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Enumerable.Empty<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }

        #region User Operations
        // Sign In and Register
        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            await _context.BeginTransactionAsync();

            try
            {
                var person = _mapper.Map<Person>(model);
                await _context.PersonRepository.AddAsync(person);
                await _context.SaveAsync();

                model.ProfilePictureUrl = await ProcessImageAsync(model);
                var user = _mapper.Map<User>(model);

                var grade = await _context.GradeRepository.GetByNameAsync("1st Grade");
                user.GradeId = grade!.Id;

                user.PersonId = person.Id;
                user.RegistrationDate = DateTime.Now;

                var registerUserResult = await _userManager.CreateAsync(user, model.Password);

                if (registerUserResult.Succeeded)
                {
                    await _context.CommitTransactionAsync();
                    await AddUserToRoleAsync(user, "User").ConfigureAwait(false);

                    return IdentityResult.Success;
                }
                else
                {
                    await _context.RollbackTransactionAsync();
                    return registerUserResult;
                }
            }
            catch (Exception e)
            {
                await _context.RollbackTransactionAsync();
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {e.Message}" });
            }
        }

        public async Task<(string token, IEnumerable<string> roles, LoginResult result)> SignInUserAsync(LoginModel model, HttpContext httpContext)
        {
            var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
            {
                return (string.Empty, Enumerable.Empty<string>(), LoginResult.InvalidCredentials);
            }

            var isDeviceRemembered = httpContext.Request.Cookies.ContainsKey("remember_device");
            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            var isUserIdMatching = httpContext.Request.Cookies.TryGetValue("twofactor-userid", out string userIdCookie) && userIdCookie == user.Id;

            if (isTwoFactorEnabled && !isUserIdMatching && !isDeviceRemembered)
            {
                return (string.Empty, Enumerable.Empty<string>(), LoginResult.RequiresTwoFactor);
            }

            var accessToken = await GenerateTokensAsync(httpContext, model.Email, model.RememberMe);
            await SetActiveStatusAsync(user.Id, true);

            var roles = await GetUserRolesAsync(user.Id);
            var userRoles = ConvertStringsToUserRoles(roles);

            return (accessToken, userRoles, LoginResult.Success);
        }

        public async Task<(string token, IEnumerable<string> roles, LoginResult result)> SignInUserWithTwoFactorAsync(HttpContext httpContext, TwoFactorModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return (string.Empty, Enumerable.Empty<string>(), LoginResult.InvalidCredentials);
            }

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, model.Code);

            if (!result)
            {
                return (string.Empty, Enumerable.Empty<string>(), LoginResult.InvalidCredentials);
            }

            if (model.RememberTwoFactorDevice)
            {
                _twoFactorService.SetTwoFactorRememberDeviceCookie(httpContext, user.Id);
            }

            var accessToken = await GenerateTokensAsync(httpContext, user.Email!, model.RememberMe);
            await SetActiveStatusAsync(user.Id, true);

            var roles = await GetUserRolesAsync(user.Id);
            var userRoles = ConvertStringsToUserRoles(roles);

            return (accessToken, userRoles, LoginResult.Success);
        }

        public async Task<IdentityResult> ConfirmRegistrationAsync(string email, string confirmationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            if (user.EmailConfirmed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Your Email Already Confirmed." });
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            return result;
        }

        public async Task<bool> ResetPasswordAsync(string email, string password, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<LockoutResult> LockOutUserAsync(LockoutModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null)
            {
                return LockoutResult.UserNotFound;
            }

            if (await _userManager.IsLockedOutAsync(user).ConfigureAwait(false))
            {
                return LockoutResult.AlreadyLockedOut;
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, model.LockoutEnd).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                return LockoutResult.LockoutFailed;
            }

            await SetActiveStatusAsync(user.Id, false);

            return LockoutResult.Success;
        }

        #endregion

        #region Access and Refresh Token Operations

        public async Task<string> GenerateTokensAsync(HttpContext httpContext, string email, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            // Access Token lifespan is to short no need to store it in db like Refresh Token
            // Generate and Set access token as HTTP-only cookie & Set access token as HTTP-only cookie
            var accessToken = await _jwtService.GenerateJwtAccessTokenAsync(user).ConfigureAwait(false);
            _jwtService.SetAccessTokenCookie(httpContext, accessToken);

            // Creating and Storing Refresh Token in UserTokens Database & Set refresh token as HTTP-only cookie
            var refreshToken = _jwtService.GenerateRefreshToken();
            await _jwtService.StoreRefreshTokenAsync(user.Id, refreshToken, rememberMe);
            _jwtService.SetRefreshTokenCookie(httpContext, refreshToken, rememberMe);

            return accessToken;
        }

        public async Task<(RefreshTokenModel? model, IEnumerable<string> roles)> GetRefreshTokenAsync(HttpContext httpContext, bool rememberMe)
        {
            var refreshToken = httpContext.Request.Cookies["RefreshToken"];

            if (refreshToken == null)
            {
                return (null, Enumerable.Empty<string>());
            }

            var user = await _jwtService.GetUserByRefreshTokenAsync(refreshToken).ConfigureAwait(false);
            if (user == null)
            {
                return (null, Enumerable.Empty<string>());
            }

            if (!await _jwtService.IsRefreshTokenValidAsync(refreshToken).ConfigureAwait(false))
            {
                return (null, Enumerable.Empty<string>());
            }

            /*

            var principal = _jwtService.GetPrincipalFromExpiredToken(httpContext.Request.Cookies["AccessToken"]);

            if (principal == null)
            {
                return null;
            }

            var principalUser = await _jwtService.GetUserFromPrincipalAsync(principal);

            if (principalUser == null)
            {
                return null;
            } */

            var newAccessToken = await _jwtService.GenerateJwtAccessTokenAsync(user).ConfigureAwait(false);
            _jwtService.SetAccessTokenCookie(httpContext, newAccessToken);

            var newRefreshToken = _jwtService.GenerateRefreshToken();
            await _jwtService.UpdateRefreshTokenAsync(user.Id, refreshToken, newRefreshToken, rememberMe).ConfigureAwait(false);
            _jwtService.SetRefreshTokenCookie(httpContext, newRefreshToken, rememberMe);

            var roles = await GetUserRolesAsync(user.Id);
            var userRoles = ConvertStringsToUserRoles(roles);

            var model = new RefreshTokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return (model, userRoles);
        }

        public async Task RemoveTokensAsync(string refreshToken, HttpContext httpContext)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _jwtService.RemoveRefreshTokenAsync(refreshToken).ConfigureAwait(false);
            }

            _jwtService.ClearAuthCookies(httpContext);
            _twoFactorService.DeleteTwoFactorRememberDeviceCookie(httpContext);
        }
        #endregion

        #region One Time Token Generation and Verification
        public async Task<string?> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (user == null || user.EmailConfirmed)
            {
                return null;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

            return token;
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (user == null)
            {
                return null;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<bool> IsTwoFactorEnabledAsync(User user)
        {
            return await _userManager.GetTwoFactorEnabledAsync(user);
        }

        public async Task SetActiveStatusAsync(string userId, bool value)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);

            if (user != null)
            {
                user.ActiveStatus = value;
                await _userManager.UpdateAsync(user).ConfigureAwait(false);
            }
        }
        #endregion

        #region Convert Roles To Hash

        public List<string> ConvertStringsToUserRoles(IEnumerable<string> roles)
        {
            Dictionary<string, string> RoleMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "User", "$2b$10$RQbxyABCDy/VHZMBsghfLO.aPDYPVPLJDRSFT1iPxP3MSUwIStEGO" },
                { "Admin", "$2b$10$2aXYuiABCDy/VHZMBsgLkO4PyXPZORPSAF3KDJTG1iNp2VCEI.dGk" },
            };

        var userRoles = new List<string>();

            foreach (var role in roles)
            {
                if (RoleMapping.TryGetValue(role, out var userRole))
                {
                    userRoles.Add(userRole);
                }
            }

            return userRoles;
        }

        #endregion

        #region Store Image Helper Method

        public async Task<string?> ProcessImageAsync(RegisterModel model)
        {
            if (model == null || model.ProfilePicture == null)
            {
                return null;
            }

            var fileName = $"{model.UserName}_{model.ProfilePicture.FileName}";
            var projectDir = Directory.GetParent(Environment.CurrentDirectory)!.FullName;
            var filePath = Path.Combine(projectDir, "mathquest.client", "public", "images", "profile", fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                var index = filePath.IndexOf("images", StringComparison.InvariantCulture);
                var relativeDbPath = filePath[index..].Replace("\\", "/", StringComparison.InvariantCulture);

                return relativeDbPath;
            }
            catch (Exception)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                return null;
            }
        }

        #endregion
    }
}
