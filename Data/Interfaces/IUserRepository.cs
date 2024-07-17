using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(string id);

        void Delete(User entity);

        Task DeleteByIdAsync(string id);

        void Update(User entity);

        // Registration & Authorization contracts
        Task<IdentityResult> AddAsync(User entity, string password);
        Task<User> FindByEmailAsync(string email);
        Task<SignInResult> SignInAsync(string email, string password, bool rememberMe);

        // Add/Remove Users to/From Roles and get User Roles
        Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName);
        Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
