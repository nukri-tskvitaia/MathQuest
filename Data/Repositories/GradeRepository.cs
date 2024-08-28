using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class GradeRepository : AbstractRepository, IGradeRepository
    {
        private readonly DbSet<Grade> _grades;

        public GradeRepository(MathQuestDbContext context)
            : base(context)
        {
            this._grades = context.Set<Grade>();
        }

        public async Task AddAsync(Grade entity)
        {
            await _grades.AddAsync(entity);
        }

        public void Delete(Grade entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Grade>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Grade> GetByIdAsync(int id)
        {
            return await _grades.FindAsync(id) ?? new Grade();
        }

        public async Task<Grade?> GetByNameAsync(string name)
        {
            return await _grades.FirstOrDefaultAsync(g => g.GradeName == name);
        }

        public void Update(Grade entity)
        {
            throw new NotImplementedException();
        }
    }
}
