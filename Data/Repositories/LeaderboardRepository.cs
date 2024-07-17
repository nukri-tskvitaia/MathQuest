using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class LeaderboardRepository : AbstractRepository, ILeaderboardRepository
    {
        private readonly DbSet<Leaderboard> _leaderboards;

        public LeaderboardRepository(MathQuestDbContext context)
            : base(context)
        {
            this._leaderboards = context.Set<Leaderboard>();
        }

        public async Task AddAsync(Leaderboard entity)
        {
            await this._leaderboards.AddAsync(entity);
        }

        public void Delete(Leaderboard entity)
        {
            _leaderboards.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _leaderboards.FindAsync(id);

            if (entity != null)
            {
                this._leaderboards.Remove(entity);
            }
        }

        public async Task<IEnumerable<Leaderboard>> GetAllAsync()
        {
            return await _leaderboards
                .Include(x => x.User)
                .OrderByDescending(x => x.Points)
                .ToListAsync();
        }

        public async Task<IEnumerable<Leaderboard>> GetAllWithPaginationAsync(int pageSize, int count)
        {
            return await _leaderboards
                .Include(x => x.User)
                .OrderByDescending(x => x.Points)
                .Skip((pageSize - 1) * count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Leaderboard> GetByIdAsync(int id)
        {
            return await _leaderboards
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id) ?? new Leaderboard();
        }

        public async Task<Leaderboard?> GetByUserIdAsync(string userId)
        {
            return await _leaderboards
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public void Update(Leaderboard entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            var existingEntity = _leaderboards.FirstOrDefault(x => x.UserId == entity.UserId);
            ArgumentNullException.ThrowIfNull(existingEntity);

            existingEntity.Points += entity.Points;
        }
    }
}
