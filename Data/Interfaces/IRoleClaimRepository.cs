using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRoleClaimRepository
    {
        Task AddRoleClaimAsync(IdentityRoleClaim<string> claim);
        Task RemoveRoleClaimAsync(IdentityRoleClaim<string> claim);
        Task<IList<IdentityRoleClaim<string>>> GetRoleClaimsAsync(string roleId);
    }
}
