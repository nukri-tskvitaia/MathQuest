using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserClaimRepository
    {
        Task AddUserClaimAsync(IdentityUserClaim<string> claim);
        Task RemoveUserClaimAsync(IdentityUserClaim<string> claim);
        Task<IList<IdentityUserClaim<string>>> GetUserClaimsAsync(string userId);
    }
}
