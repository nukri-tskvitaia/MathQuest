using Data.Data;
using Data.Entities.Identity;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : AbstractRepository, IUserRepository
    {
        private readonly DbSet<User> _users;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(MathQuestDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
            : base(context)
        {
            this._users = context.Set<User>();
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public void Delete(User entity)
        {
            this._users.Remove(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var entity = await this._users.FindAsync(id);

            if (entity != null)
            {
                this._users.Remove(entity);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this._users.Include(p => p.Person).ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await this._users.FindAsync(id) ?? new User();
        }

        public void Update(User entity)
        {
            if (entity != null)
            {
                var existingEntity = this._users.Find(entity.Id);

                if (existingEntity != null)
                {
                    this._context.Entry(existingEntity).State = EntityState.Detached;
                    this._users.Update(entity);
                }
            }
        }

        public async Task<IdentityResult> AddAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
        }
        
        // Users & Roles
        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }
    }
}
