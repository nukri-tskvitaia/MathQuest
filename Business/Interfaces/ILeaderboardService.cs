using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface ILeaderboardService
    {
        public Task<bool> AddUserPointsAsync(LeaderboardDataModel model);
        public Task<IEnumerable<LeaderboardDataModel>> GetBestUserScoresAsync(int pageSize = 1, int count = 20);
        public Task<LeaderboardDataModel> GetUserScoreAsync(string userId);
    }
}
