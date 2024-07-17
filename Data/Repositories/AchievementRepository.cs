using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class AchievementRepository : AbstractRepository, IAchievementRepository
    {
        private readonly DbSet<Achievement> _achievements;

        public AchievementRepository(MathQuestDbContext context)
            : base(context)
        {
            this._achievements = context.Set<Achievement>();
        }

        public async Task AddAsync(Achievement entity)
        {
            await this._achievements.AddAsync(entity);
        }

        public void Delete(Achievement entity)
        {
            this._achievements.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await this._achievements.FindAsync(id);

            if (entity != null)
            {
                this._achievements.Remove(entity);
            }
        }

        public async Task<IEnumerable<Achievement>> GetAllAsync()
        {
            return await this._achievements.ToListAsync();
        }

        public async Task<Achievement> GetByIdAsync(int id)
        {
            return await this._achievements.FindAsync(id) ?? new Achievement();
        }

        public void Update(Achievement entity)
        {
            if (entity != null)
            {
                var existingEntity = this._achievements.Find(entity.Id);

                if (existingEntity != null)
                {
                    this._context.Entry(existingEntity).State = EntityState.Detached;
                    this._achievements.Update(entity);
                }
            }
        }
    }
}
