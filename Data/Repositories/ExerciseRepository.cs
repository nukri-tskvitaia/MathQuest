using Data.Data;
using Data.Entities;
using Data.Entities.Identity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ExerciseRepository : AbstractRepository, IExerciseRepository
    {
        private readonly DbSet<Exercise> _exercises;

        public ExerciseRepository(MathQuestDbContext context)
            : base(context)
        {
            this._exercises = context.Set<Exercise>();
        }

        public async Task AddAsync(Exercise entity)
        {
            await this._exercises.AddAsync(entity);
        }

        public void Delete(Exercise entity)
        {
            this._exercises.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await this._exercises.FindAsync(id);

            if (entity != null)
            {
                this._exercises.Remove(entity);
            }
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await this._exercises.ToListAsync();
        }

        public async Task<Exercise> GetByIdAsync(int id)
        {
            return await this._exercises.FindAsync(id) ?? new Exercise();
        }

        public void Update(Exercise entity)
        {
            if (entity != null)
            {
                var existingEntity = this._exercises.Find(entity.Id);

                if (existingEntity != null)
                {
                    this._context.Entry(existingEntity).State = EntityState.Detached;
                    this._exercises.Update(entity);
                }
            }
        }
    }
}
