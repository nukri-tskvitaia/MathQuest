using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserExerciseRepository : AbstractRepository, IUserExerciseRepository
    {
        private readonly DbSet<UserExercise> _userExercises;

        public UserExerciseRepository(MathQuestDbContext context)
            : base(context)
        {
            this._userExercises = context.Set<UserExercise>();
        }

        public Task AddAsync(UserExercise entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserExercise entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserExercise>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserExercise> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserExercise entity)
        {
            throw new NotImplementedException();
        }
    }
}
