using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Interfaces
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        public Task<Feedback?> GetByUserAndFeedbackIdsAsync(string userId, int feedbackId);
        public Task<IEnumerable<Feedback>> GetUserFeedbacksByUserIdAsync(string userId, DateTime fromDate, DateTime toDate);
        public void DeleteRange(IEnumerable<Feedback> feedbacks);
    }
}