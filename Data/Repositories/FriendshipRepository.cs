using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class FriendshipRepository : AbstractRepository, IFriendshipRepository
    {
        private readonly DbSet<Friend> _friendships;

        public FriendshipRepository(MathQuestDbContext context)
            : base(context)
        {
            this._friendships = context.Set<Friend>();
        }

        public async Task AddAsync(Friend entity)
        {
            await _friendships.AddAsync(entity);
        }

        public void Delete(Friend entity)
        {
            _context.Remove(entity);
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Friend>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Friend?>> GetAllPendingRequestsAsync(string userId)
        {
            var pendingFriends = await _friendships
                .Include(x => x.RequestedBy)
                .Where(x => x.RequestedToId == userId && x.FriendRequestStatus == FriendRequestStatus.Pending)
                .ToListAsync();
            /*
            var pendingRequests = pendingFriends
                .Select(x => x.RequestedBy!.UserName!); */

            return pendingFriends;
        }

        public async Task<Friend?> GetRequestFriendAsync(string userId, string otherUserId)
        {
            var pendingFriend = await _friendships
                .FirstOrDefaultAsync(f => f.RequestedById == otherUserId && f.RequestedToId == userId);

            return pendingFriend;
        }

        public async Task<FriendRequestStatus?> GetFriendRequestStatusAsync(string userId, string otherUserId)
        {
            var friendship = await _friendships
                .FirstOrDefaultAsync(f => (f.RequestedById == userId && f.RequestedToId == otherUserId) 
                || (f.RequestedById == otherUserId && f.RequestedToId == userId));

            if (friendship == null)
            {
                return FriendRequestStatus.None;
            }

            var status = friendship.FriendRequestStatus;

            return status;
        }

        public Task<bool> ConfirmFriend(Friend entity)
        {
            entity.FriendRequestStatus = FriendRequestStatus.Approved;
            entity.BecameFriendsDate = DateTime.Now;

            return Task.FromResult(true);
        }

        public Task<bool> RejectFriend(Friend entity)
        {
            _context.Remove(entity);

            return Task.FromResult(true);
        }

        public async Task<Friend?> GetByIdsAsync(string userId, string otherUserId)
        {
            var friendship = await _friendships
                .Where(x => x.RequestedById == userId && x.RequestedToId == otherUserId)
                .FirstOrDefaultAsync();

            return friendship;
        }

        public async Task<Friend?> GetByUsernamesAsync(string username, string otherUserName)
        {
            var friendship = await _friendships
                .Include(x => x.RequestedBy)
                .Include(x => x.RequestedTo)
                .Where(x => x.RequestedBy.UserName == username && x.RequestedTo.UserName == otherUserName)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            return friendship;
        }

        public void Update(Friend entity)
        {
            throw new NotImplementedException();
        }
    }
}
