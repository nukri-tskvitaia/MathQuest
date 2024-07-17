using Business.Interfaces;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserRoleAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{userId}' not found." });
            }

            return IdentityResult.Success;
        }

        public Task<IdentityUserRole<string>> GetUserRoleAsync(string userId, string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityUserRole<string>>> GetUserRolesByRoleIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityUserRole<string>>> GetUserRolesByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserRoleAsync(string userId, string roleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserRoleByRoleIdAsync(string userId, string roleId, string oldRoleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserRoleByUserIdAsync(string userId, string roleId, string oldUserId)
        {
            throw new NotImplementedException();
        }
    }
}
