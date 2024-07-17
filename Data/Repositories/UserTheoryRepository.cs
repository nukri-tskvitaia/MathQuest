using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserTheoryRepository : AbstractRepository, IUserTheoryRepository
    {
        private readonly DbSet<UserTheory> _userTheories;

        public UserTheoryRepository(Data.MathQuestDbContext context)
            : base(context)
        {
            this._userTheories = context.Set<UserTheory>();
        }

        public Task AddAsync(UserTheory entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserTheory entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserTheory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserTheory> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserTheory entity)
        {
            throw new NotImplementedException();
        }
    }
}
