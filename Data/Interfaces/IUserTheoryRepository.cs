using Data.Entities;

namespace Data.Interfaces
{
    public interface IUserTheoryRepository
    {
        Task<IEnumerable<UserTheory>> GetAllAsync();

        Task<UserTheory> GetByIdAsync(int id);

        Task AddAsync(UserTheory entity);

        void Delete(UserTheory entity);

        Task DeleteByIdAsync(int id);

        void Update(UserTheory entity);
    }
}
