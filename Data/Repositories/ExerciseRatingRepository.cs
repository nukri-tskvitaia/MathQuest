using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ExerciseRatingRepository : AbstractRepository, IExerciseRatingRepository
    {
        private readonly DbSet<ExerciseRating> _exerciseRatings;

        public ExerciseRatingRepository(MathQuestDbContext context)
            : base(context)
        {
            this._exerciseRatings = context.Set<ExerciseRating>();
        }

        public async Task AddAsync(ExerciseRating entity)
        {
            await this._exerciseRatings.AddAsync(entity);
        }

        public void Delete(ExerciseRating entity)
        {
            this._exerciseRatings.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await this._exerciseRatings.FindAsync(id);

            if (entity != null)
            {
                this._exerciseRatings.Remove(entity);
            }
        }

        public async Task<IEnumerable<ExerciseRating>> GetAllAsync()
        {
            return await this._exerciseRatings.ToListAsync();
        }

        public async Task<ExerciseRating> GetByIdAsync(int id)
        {
            return await this._exerciseRatings.FindAsync(id) ?? new ExerciseRating();
        }

        public void Update(ExerciseRating entity)
        {
            if (entity != null)
            {
                var existingEntity = this._exerciseRatings.Find(entity.UserId, entity.ExerciseId);

                if (existingEntity != null)
                {
                    this._context.Entry(existingEntity).State = EntityState.Detached;
                    this._exerciseRatings.Update(entity);
                }
            }
        }
    }
}
