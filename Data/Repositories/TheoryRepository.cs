using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TheoryRepository : AbstractRepository, ITheoryRepository
    {
        private readonly DbSet<Theory> _theories;

        public TheoryRepository(MathQuestDbContext context)
            : base(context)
        {
            this._theories = context.Set<Theory>();
        }

        public Task AddAsync(Theory entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Theory entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Theory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Theory> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Theory entity)
        {
            throw new NotImplementedException();
        }
    }
}
