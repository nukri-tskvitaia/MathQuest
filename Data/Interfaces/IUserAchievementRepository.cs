using Data.Entities;

namespace Data.Interfaces
{
    public interface IUserAchievementRepository
    {
        Task<IEnumerable<UserAchievement>> GetAllAsync();

        Task<UserAchievement> GetByIdAsync(int id);

        Task AddAsync(UserAchievement entity);

        void Delete(UserAchievement entity);

        Task DeleteByIdAsync(int id);

        void Update(UserAchievement entity);
    }
}
