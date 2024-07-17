using Data.Entities;

namespace Data.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message entity);

        Task<IEnumerable<IEnumerable<Message>>> GetAllMessagesAsync(int pageNumber = 1, int pageSize = 10, int limitGroups = 10);

        Task<IEnumerable<IEnumerable<Message>>> GetUsersAllMessagesAsync(string userId, int pageNumber = 1, int pageSize = 10, int limitGroups = 10);
        
        Task<IEnumerable<Message>> GetMessagesByIdsAsync(string senderId, string receiverId);

        public Task<Message?> GetMessageByIdsAsync(string senderId, string receiverId, int messageId);

        void Delete(Message entity);

        Task DeleteMessagesByIdsAsync(string senderId, string receiverId);

        public Task DeleteMessageByIdsAsync(string senderId, string receiverId, int messageId);

        void Update(Message entity);
    }
}
