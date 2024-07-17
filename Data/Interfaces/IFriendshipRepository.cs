using Data.Entities;

namespace Data.Interfaces
{
    public interface IFriendshipRepository
    {
        Task<IEnumerable<Friend>> GetAllAsync();

        Task<Friend?> GetByIdsAsync(string userId, string otherUserId);

        Task<Friend?> GetByUsernamesAsync(string username, string otherUserName);

        public Task<IEnumerable<Friend?>> GetAllPendingRequestsAsync(string userId);

        public Task<Friend?> GetRequestFriendAsync(string userId, string otherUserId);

        public Task<FriendRequestStatus?> GetFriendRequestStatusAsync(string userId, string otherUserId);

        public Task<bool> ConfirmFriend(Friend entity);

        public Task<bool> RejectFriend(Friend entity);

        Task AddAsync(Friend entity);

        void Delete(Friend entity);

        Task DeleteByIdAsync(int id);

        void Update(Friend entity);
    }
}
