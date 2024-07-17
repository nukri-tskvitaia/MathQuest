using Data.Entities;

namespace Data.Interfaces
{
    public interface IExerciseRatingRepository
    {
        Task<IEnumerable<ExerciseRating>> GetAllAsync();

        Task<ExerciseRating> GetByIdAsync(int id);

        Task AddAsync(ExerciseRating entity);

        void Delete(ExerciseRating entity);

        Task DeleteByIdAsync(int id);

        void Update(ExerciseRating entity);
    }
}
