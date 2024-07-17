using Data.Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRoleRepository
    {
        private readonly DbSet<IdentityUserRole<string>> _userRoles;

        public UserRoleRepository(MathQuestDbContext context)
        {
            _userRoles = context.Set<IdentityUserRole<string>>();
        }

        public async Task<IdentityUserRole<string>> GetUserRoleAsync(string userId, string roleId)
        {
            return await _userRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task<IEnumerable<IdentityUserRole<string>>> GetUserRolesByUserIdAsync(string userId)
        {
            return await _userRoles.Where(ur => ur.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<IdentityUserRole<string>>> GetUserRolesByRoleIdAsync(string roleId)
        {
            return await _userRoles.Where(ur => ur.RoleId == roleId).ToListAsync();
        }

        public async Task AddUserRoleAsync(string userId, string roleId)
        {
            var userRole = new IdentityUserRole<string> { UserId = userId, RoleId = roleId };
            await _userRoles.AddAsync(userRole);
        }

        public async Task RemoveUserRoleAsync(string userId, string roleId)
        {
            var userRole = await _userRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (userRole != null)
            {
                _userRoles.Remove(userRole);
            }
        }

        public async Task UpdateUserRoleByUserIdAsync(string userId, string roleId, string oldUserId)
        {
            var userRole = await GetUserRoleAsync(userId, oldUserId);
            if (userRole != null)
            {
                userRole.UserId = userId;
                _userRoles.Update(userRole);
            }
        }

        public async Task UpdateUserRoleByRoleIdAsync(string userId, string roleId, string oldRoleId)
        {
            var userRole = await GetUserRoleAsync(userId, oldRoleId);
            if (userRole != null)
            {
                userRole.RoleId = roleId;
                _userRoles.Update(userRole);
            }
        }
    }
}
