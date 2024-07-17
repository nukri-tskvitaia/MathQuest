using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;

        public FeedbackService(IMapper mapper, IUnitOfWork context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task AddFeedbackAsync(FeedbackModel feedback)
        {
            var model = _mapper.Map<Feedback>(feedback);
            await _context.FeedbackRepository.AddAsync(model);
            await _context.SaveAsync();
        }

        public async Task DeleteFeedbackByUserIdAsync(string userId, int feedbackId)
        {
            var feedback = await _context.FeedbackRepository
                .GetByUserAndFeedbackIdsAsync(userId, feedbackId).ConfigureAwait(false);

            if (feedback == null)
            {
                throw new MathQuestException();
            }

            _context.FeedbackRepository.Delete(feedback);
            await _context.SaveAsync();
        }

        public async Task DeleteUserFeedbacksInPeriodAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.FeedbackRepository
                .GetUserFeedbacksByUserIdAsync(userId, fromDate, toDate).ConfigureAwait(false);

            try
            {
                _context.FeedbackRepository.DeleteRange(result);
            }
            catch (Exception)
            {
                throw new MathQuestException();
            }
        }

        public async Task<IEnumerable<FeedbackModel>> GetAllFeedbacksAsync(DateTime fromDate, DateTime toDate)
        {
            var feedbacks = await _context.FeedbackRepository
                .GetAllAsync().ConfigureAwait(false);
            var filteredFeedbacks = feedbacks
                .Where(x => x.FeedbackDate >= fromDate && x.FeedbackDate <= toDate);
            
            List<FeedbackModel> feedbackModels = new List<FeedbackModel>();
            foreach (var feedback in filteredFeedbacks)
            {
                feedbackModels.Add(_mapper.Map<FeedbackModel>(feedback));
            }

            return feedbackModels;
        }

        public async Task<IEnumerable<FeedbackModel>> GetUsersAllFeedbacksByUserIdAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            var userFeedbacks = await _context.FeedbackRepository
                .GetUserFeedbacksByUserIdAsync(userId, fromDate, toDate);
            var filteredUserFeedbacks = userFeedbacks
                .Where(x => x.FeedbackDate > fromDate && x.FeedbackDate < toDate);

            List<FeedbackModel> feedbackModels = new List<FeedbackModel>();
            foreach (var feedback in filteredUserFeedbacks)
            {
                feedbackModels.Add(_mapper.Map<FeedbackModel>(feedback));
            }

            return feedbackModels;
        }

        public async Task<FeedbackModel> GetSingleUserFeedbackAsync(string userId, int feedbackId)
        {
            var feedback = await _context.FeedbackRepository
                .GetByUserAndFeedbackIdsAsync(userId, feedbackId)
                .ConfigureAwait(false);

            if (feedback == null)
            {
                throw new MathQuestException();
            }

            return _mapper.Map<FeedbackModel>(feedback);
        }
    }
}
