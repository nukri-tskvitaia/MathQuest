using Data.Entities;

namespace Data.Interfaces
{
    public interface IGradeRepository : IRepository<Grade>
    {
        public Task<Grade?> GetByNameAsync(string name);
    }
}
