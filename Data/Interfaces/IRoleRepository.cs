using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<IdentityRole> FindRoleByIdAsync(string roleId);
        Task<IdentityRole> FindRoleByNameAsync(string roleName);
        Task<IList<IdentityRole>> GetAllRolesAsync();
    }
}
