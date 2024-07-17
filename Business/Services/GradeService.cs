using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _context;

        public GradeService(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<Grade?> GetGradeByIdAsync(int id)
        {
            var grade = await _context.GradeRepository.GetByIdAsync(id);

            return grade;
        }

        public async Task<int?> GetGradeLevelByIdAsync(int id)
        {
            var grade = await GetGradeByIdAsync(id);

            return grade?.GradeLevel;
        }

        public async Task<string?> GetGradeNameByIdAsync(int id)
        {
            var grade = await GetGradeByIdAsync(id);

            return grade?.GradeName;
        }
    }
}
