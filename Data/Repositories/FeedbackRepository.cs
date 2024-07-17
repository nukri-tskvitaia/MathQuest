using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class FeedbackRepository : AbstractRepository, IFeedbackRepository
    {
        private readonly DbSet<Feedback> _feedbacks;

        public FeedbackRepository(MathQuestDbContext context)
            : base(context)
        {
            this._feedbacks = context.Set<Feedback>();
        }

        public async Task AddAsync(Feedback entity)
        {
            await this._feedbacks.AddAsync(entity);
        }

        public void Delete(Feedback entity)
        {
            this._feedbacks.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await this._feedbacks.FindAsync(id);

            if (entity != null)
            {
                this._feedbacks.Remove(entity);
            }
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await this._feedbacks.ToListAsync();
        }

        public async Task<Feedback> GetByIdAsync(int id)
        {
            return await this._feedbacks.FindAsync(id) ?? new Feedback();
        }

        public async Task<IEnumerable<Feedback>> GetUserFeedbacksByUserIdAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            return await this._feedbacks
                .Where(x => x.UserId == userId && fromDate <= x.FeedbackDate && toDate >= x.FeedbackDate)
                .ToListAsync();
        }

        public async Task<Feedback?> GetByUserAndFeedbackIdsAsync(string userId, int feedbackId)
        {
            return await this._feedbacks
                .Where(x => x.UserId == userId && x.Id == feedbackId)
                .FirstOrDefaultAsync();
        }

        public void Update(Feedback entity)
        {
            if (entity != null)
            {
                var existingEntity = this._feedbacks.Find(entity.Id);

                if (existingEntity != null)
                {
                    this._context.Entry(existingEntity).State = EntityState.Detached;
                    this._feedbacks.Update(entity);
                }
            }
        }

        public void DeleteRange(IEnumerable<Feedback> feedbacks)
        {
            _feedbacks.RemoveRange(feedbacks);
        }
    }
}
