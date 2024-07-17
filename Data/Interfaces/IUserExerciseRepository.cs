using Data.Entities;

namespace Data.Interfaces
{
    public interface IUserExerciseRepository
    {
        Task<IEnumerable<UserExercise>> GetAllAsync();

        Task<UserExercise> GetByIdAsync(int id);

        Task AddAsync(UserExercise entity);

        void Delete(UserExercise entity);

        Task DeleteByIdAsync(int id);

        void Update(UserExercise entity);
    }
}
