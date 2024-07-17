using Data.Entities;

namespace Data.Interfaces
{
    public interface ITheoryRatingRepository
    {
        Task<IEnumerable<TheoryRating>> GetAllAsync();

        Task<TheoryRating> GetByIdAsync(int id);

        Task AddAsync(TheoryRating entity);

        void Delete(TheoryRating entity);

        Task DeleteByIdAsync(int id);

        void Update(TheoryRating entity);
    }
}
