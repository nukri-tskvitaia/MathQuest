using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserTokenRepository
    {
        Task AddUserTokenAsync(IdentityUserToken<string> token);
        Task RemoveUserTokenAsync(IdentityUserToken<string> token);
        Task<IdentityUserToken<string>> FindUserTokenAsync(string userId, string loginProvider, string name);
    }
}
