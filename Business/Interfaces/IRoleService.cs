using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(string roleName);
        
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityResult> DeleteRoleAsync(string roleName);

        Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName);

        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName);

        Task<IList<string>> GetUserRolesAsync(string userId);

        Task<IList<IdentityRole>> GetAllRolesAsync();

        Task<IdentityRole> FindRoleByIdAsync(string roleId);

        Task<IdentityRole> FindRoleByNameAsync(string roleName);
    }
}
