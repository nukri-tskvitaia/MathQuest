using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TheoryRatingRepository : AbstractRepository, ITheoryRatingRepository
    {
        private readonly DbSet<TheoryRating> _theoryRatings;

        public TheoryRatingRepository(MathQuestDbContext context)
            : base(context)
        {
            this._theoryRatings = context.Set<TheoryRating>();
        }

        public Task AddAsync(TheoryRating entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TheoryRating entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TheoryRating>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TheoryRating> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TheoryRating entity)
        {
            throw new NotImplementedException();
        }
    }
}
