using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IMessageService
    {
        Task AddMessageAsync(MessageModel model);

        // for admins to see all the messages there is
        Task<IEnumerable<IEnumerable<MessageModel>>> GetAllUsersMessagesAsync(int pageNumber = 1, int pageSize = 10, int limitGroups = 10);

        Task<IEnumerable<IEnumerable<MessageModel>>> GetUsersAllMessagesAsync(string userId, int pageNumber = 1, int pageSize = 10, int limitGroups = 10);

        Task<IEnumerable<MessageModel>> GetUsersMessagesAsync(string senderId, string receiverId);

        Task<MessageModel> GetMessageByIdsAsync(string senderId, string receiverId, int messageId);

        void Delete(Message entity);

        Task DeleteMessagesByIdsAsync(string senderId, string receiverId);

        Task DeleteMessageByIdsAsync(string senderId, string receiverId, int messageId);

        void Update(Message entity);
    }
}
