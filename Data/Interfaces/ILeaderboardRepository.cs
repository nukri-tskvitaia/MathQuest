using Data.Entities;

namespace Data.Interfaces
{
    public interface ILeaderboardRepository : IRepository<Leaderboard>
    {
        public Task<IEnumerable<Leaderboard>> GetAllWithPaginationAsync(int pageSize, int count);
        public Task<Leaderboard?> GetByUserIdAsync(string  userId);
    }
}
