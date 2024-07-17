using Data.Entities;

namespace Business.Interfaces
{
    public interface IGradeService
    {
        Task<Grade?> GetGradeByIdAsync(int id);
        Task<string?> GetGradeNameByIdAsync(int id);
        Task<int?> GetGradeLevelByIdAsync(int id);
    }
}
