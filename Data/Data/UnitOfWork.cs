using Data.Entities.Identity;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Data
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MathQuestDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(MathQuestDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _userManager, _signInManager);

        public IPersonRepository PersonRepository => new PersonRepository(_context);

        public IRoleRepository RoleRepository => new RoleRepository(_roleManager);

        public IGradeRepository GradeRepository => new GradeRepository(_context);

        public IFriendshipRepository FriendshipRepository => new FriendshipRepository(_context);

        public IMessageRepository MessageRepository => new MessageRepository(_context);

        public ILeaderboardRepository LeaderboardRepository => new LeaderboardRepository(_context);

        public IFeedbackRepository FeedbackRepository => new FeedbackRepository(_context);

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
