using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserAchievementRepository : AbstractRepository, IUserAchievementRepository
    {
        private readonly DbSet<UserAchievement> _userAchievements;

        public UserAchievementRepository(MathQuestDbContext context)
            : base(context)
        {
            this._userAchievements = context.Set<UserAchievement>();
        }

        public Task AddAsync(UserAchievement entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserAchievement entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserAchievement>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserAchievement> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserAchievement entity)
        {
            throw new NotImplementedException();
        }
    }
}
