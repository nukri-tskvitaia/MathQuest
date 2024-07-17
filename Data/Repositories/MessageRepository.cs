using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class MessageRepository : AbstractRepository, IMessageRepository
    {
        private readonly DbSet<Message> _messages;

        public MessageRepository(MathQuestDbContext context)
            : base(context)
        {
            this._messages = context.Set<Message>();
        }

        // adds single message between two users
        public async Task AddMessageAsync(Message entity)
        {
            await _messages.AddAsync(entity);
        }

        // deletes single message
        public void Delete(Message entity)
        {
            _messages.Remove(entity);
        }

        // deletes whole message history between two users ( for one users end)
        public async Task DeleteMessagesByIdsAsync(string senderId, string receiverId)
        {
            var messages = await _messages
                .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId)
                .ToListAsync();

            foreach (var message in messages)
            {
                _messages.Remove(message);
            }
        }

        // deletes a single message between users
        public async Task DeleteMessageByIdsAsync(string senderId, string receiverId, int messageId)
        {
            var message = await _messages
                .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId && x.Id == messageId)
                .FirstOrDefaultAsync();

            if (message != null)
            {
                _messages.Remove(message);
            }
        }

        // this is only for admins
        // get all the messages grouped by user pairs (meaning all the recent messages between any two users)
        public async Task<IEnumerable<IEnumerable<Message>>> GetAllMessagesAsync(int pageNumber = 1, int pageSize = 10, int limitGroups= 10)
        {
            var messageGroups = await _messages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .GroupBy(x => new
                {
                    SenderId = x.SenderId.CompareTo(x.ReceiverId) < 0 ? x.SenderId : x.ReceiverId,
                    ReceiverId = x.SenderId.CompareTo(x.ReceiverId) < 0 ? x.ReceiverId : x.SenderId
                })
                .OrderByDescending(g => g.Max(m => m.SentDate)) // Order groups by the most recent message in each group
                .Take(limitGroups) // Limit the number of message groups
                .Select(g => g.OrderByDescending(m => m.SentDate)
                .Skip((pageNumber - 1) * pageSize) // skip first number of messages...
                .Take(pageSize)) // Take top messages for each group and limit messages per group
                .ToListAsync();

            return messageGroups;
        }

        // this is for logged in user
        // get all the messages grouped by pairs (meaning all the recent messages current and any other user)
        public async Task<IEnumerable<IEnumerable<Message>>> GetUsersAllMessagesAsync(string userId, int pageNumber = 1, int pageSize = 10, int limitGroups = 10)
        {
            var messageGroups = await _messages
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                .GroupBy(x => new
                {
                    SenderId = x.SenderId.CompareTo(x.ReceiverId) < 0 ? x.SenderId : x.ReceiverId,
                    ReceiverId = x.SenderId.CompareTo(x.ReceiverId) < 0 ? x.ReceiverId : x.SenderId
                })
                .OrderByDescending(g => g.Max(m => m.SentDate)) // Order groups by the most recent message in each group
                .Take(limitGroups) // Limit the number of message groups
                .Select(g => g.OrderByDescending(m => m.SentDate)
                .Skip((pageNumber - 1) * pageSize) // skip first number of messages...
                .Take(pageSize)) // Take top messages for each group and limit messages per group
                .ToListAsync();

            return messageGroups;
        }

        // getting all the messages between two users
        public async Task<IEnumerable<Message>> GetMessagesByIdsAsync(string senderId, string receiverId)
        {
            var messages = await _messages
                .Where(x => (x.SenderId == senderId && x.ReceiverId == receiverId) 
                || (x.SenderId == receiverId && x.ReceiverId == senderId))
                .OrderByDescending(x => x.SentDate)
                .ToListAsync();

            return messages;
        }

        // get single message between two users
        public async Task<Message?> GetMessageByIdsAsync(string senderId, string receiverId, int messageId)
        {
            var message = await _messages
                .Where(x => (x.SenderId == senderId && x.ReceiverId == receiverId)
                || (x.SenderId == receiverId && x.ReceiverId == senderId) && (x.Id == messageId))
                .FirstOrDefaultAsync();

            return message;
        }

        // updating just single message (two users can have many different messages)
        public void Update(Message entity)
        {
            _messages.Update(entity);
        }
    }
}
