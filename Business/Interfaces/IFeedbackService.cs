using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IFeedbackService
    {
        public Task<IEnumerable<FeedbackModel>> GetAllFeedbacksAsync(DateTime fromDate, DateTime toDate);
        public Task<IEnumerable<FeedbackModel>> GetUsersAllFeedbacksByUserIdAsync(string userId, DateTime fromDate, DateTime toDate);
        public Task<FeedbackModel> GetSingleUserFeedbackAsync(string userId, int feedbackId);
        public Task AddFeedbackAsync(FeedbackModel feedback);
        public Task DeleteUserFeedbacksInPeriodAsync(string userId, DateTime fromDate, DateTime toDate);
        public Task DeleteFeedbackByUserIdAsync(string userId, int feedbackId);
    }
}
