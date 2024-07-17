using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserLoginRepository
    {
        Task AddUserLoginAsync(IdentityUserLogin<string> login);
        Task RemoveUserLoginAsync(IdentityUserLogin<string> login);
        Task<IdentityUserLogin<string>> FindUserLoginAsync(string loginProvider, string providerKey);
        Task<IList<IdentityUserLogin<string>>> GetUserLoginsAsync(string userId);
    }
}
