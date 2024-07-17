using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IFriendshipService
    {
        public Task<List<FriendInfoExtendedModel>?> GetAllFriendsAsync(string userId);
        public Task<List<FriendInfoModel>?> GetPendingRequestsAsync(string userId);
        public Task<Friend?> GetRequestUserAsync(string userId, string otherUserId);
        public Task<bool> ConfirmFriendRequestAsync(Friend friendship);
        public Task<bool> RejectFriendRequestAsync(Friend friendship);
        public Task<FriendInfoModel?> GetFriendInfoAsync(string username);
        public Task<FriendInfoModel?> GetFriendInfoByIdAsync(string id);
        public Task<bool> AddFriendRequestAsync(string userId, string otherUserId);
        public Task<string?> GetFriendStatusAsync(string userId, string otherUserId);
        public Task<bool> RemoveFriendRequestAsync(string userId, string otherUserId);
        public Task<bool> AreFriendsAsync(string userId, string otherUserId);
    }
}
