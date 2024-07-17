using Data.Entities.Identity;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Business.Interfaces;
using AutoMapper;
using Business.Models;

namespace Business.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public FriendshipService(UserManager<User> userManager, IUnitOfWork context, IMapper mapper, IUserService userService) 
        { 
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<bool> AddFriendRequestAsync(string userId, string otherUserId)
        {
            var status = await GetFriendStatusAsync(userId, otherUserId);

            if (status != "None")
            {
                return false;
            }

            var friend = new Friend
            {
                RequestedById = userId,
                RequestedToId = otherUserId,
                RequestTime = DateTime.Now,
                FriendRequestStatus = FriendRequestStatus.Pending,
                RequestedBy = await _context.UserRepository.GetByIdAsync(userId),
                RequestedTo = await _context.UserRepository.GetByIdAsync(otherUserId)
            };

            await _context.FriendshipRepository.AddAsync(friend);
            await _context.SaveAsync();

            return true;
        }

        public async Task<List<FriendInfoExtendedModel>?> GetAllFriendsAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(x => x.SentFriendRequests!)
                    .ThenInclude(u => u.RequestedTo)
                .Include(x => x.ReceivedFriendRequests!)
                    .ThenInclude(u => u.RequestedBy)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }

            var sentFriendRequests = user.SentFriendRequests!
                .Where(fr => fr.FriendRequestStatus == FriendRequestStatus.Approved)
                .Select(fr => fr.RequestedTo)
                .Where(u => u != null)
                .ToList();
            var receivedFriendRequests = user.ReceivedFriendRequests!
                .Where(fr => fr.FriendRequestStatus == FriendRequestStatus.Approved)
                .Select(fr => fr.RequestedBy)
                .Where(u => u != null)
                .ToList();
            var friends = sentFriendRequests.Concat(receivedFriendRequests).Distinct().ToList();

            List<FriendInfoExtendedModel>? friendInfoModel = [];
            foreach (var friend in friends)
            {
                friendInfoModel.Add(_mapper.Map<FriendInfoExtendedModel>(friend));
            }

            return friendInfoModel;
        }

        public async Task<List<FriendInfoModel>?> GetPendingRequestsAsync(string userId)
        {
            var pendingRequests = await _context.FriendshipRepository.GetAllPendingRequestsAsync(userId);
            List<FriendInfoModel>? friendInfoModel = [];

            if (!pendingRequests.Any())
            {
                return friendInfoModel;
            }
            
            var users = pendingRequests.Select(x => x!.RequestedBy).ToList();
            foreach (var user in users)
            {
                friendInfoModel.Add(_mapper.Map<FriendInfoModel>(user));
            }

            return friendInfoModel;
        }

        public async Task<Friend?> GetRequestUserAsync(string userId, string otherUserId)
        {
            var friendship = await _context.FriendshipRepository.GetRequestFriendAsync(userId, otherUserId);

            return friendship;
        }

        public async Task<bool> ConfirmFriendRequestAsync(Friend friendship)
        {
            var result = await _context.FriendshipRepository.ConfirmFriend(friendship);
            await _context.SaveAsync();

            return result;
        }

        public async Task<bool> RejectFriendRequestAsync(Friend friendship)
        {
            var result = await _context.FriendshipRepository.RejectFriend(friendship);
            await _context.SaveAsync();

            return result;
        }

        public async Task<FriendInfoModel?> GetFriendInfoAsync(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return null;
            }

            var userInfo = _mapper.Map<FriendInfoModel>(user);

            return userInfo;
        }

        public async Task<FriendInfoModel?> GetFriendInfoByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            var userInfo = _mapper.Map<FriendInfoModel>(user);

            return userInfo;
        }

        public async Task<string?> GetFriendStatusAsync(string userId, string otherUserId)
        {
            var status = await _context.FriendshipRepository.GetFriendRequestStatusAsync(userId, otherUserId);
            var result = ConvertFriendStatusEnumToString(status);

            return result;
        }

        public async Task<bool> AreFriendsAsync(string userId, string otherUserId)
        {
            var result = await GetFriendStatusAsync(userId, otherUserId).ConfigureAwait(false);

            if (result != "Approved")
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveFriendRequestAsync(string userId, string otherUserId)
        {
            var friendship = await _context.FriendshipRepository.GetByIdsAsync(userId, otherUserId);

            if (friendship == null)
            {
                return false;
            }

            _context.FriendshipRepository.Delete(friendship);
            await _context.SaveAsync();

            return true;
        }

        #region Static Helpers

        public string ConvertFriendStatusEnumToString(FriendRequestStatus? status)
        {
            Dictionary<FriendRequestStatus, string> statuses = new Dictionary<FriendRequestStatus, string>()
            {
                { FriendRequestStatus.None, "None" },
                { FriendRequestStatus.Pending, "Pending" },
                { FriendRequestStatus.Approved, "Approved" },
                { FriendRequestStatus.Rejected, "Rejected" },
            };

            if (status == null)
            {
                return string.Empty;
            }
            
            if (statuses.TryGetValue((FriendRequestStatus)status, out var result))
            {
                return result;
            }

            return string.Empty;
        }

        #endregion
    }
}
