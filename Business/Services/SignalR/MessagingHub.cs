using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Business.Services.SignalR
{
    [Authorize]
    public class MessagingHub : Hub
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public MessagingHub(IFriendshipService friendshipService, IUserService userService, IMessageService messageService)
        {
            _friendshipService = friendshipService;
            _userService = userService;
            _messageService = messageService;
        }

        public async Task SendMessageAsync(string receiverUserName, string message)
        {
            var userId = Context.UserIdentifier 
                ?? throw new MathQuestException("User/Users not found");
            
            var user = await _userService.GetUserByIdAsync(userId);
            var friend = await _userService.GetUserByUsernameAsync(receiverUserName);

            if (friend == null || user == null)
            {
                throw new MathQuestException("User/Users not found");
            }

            var messageModel = new MessageModel
            {
                SenderId = userId,
                ReceiverId = friend.Id,
                SentDate = DateTime.Now,
                Text = message
            };

            if (await _friendshipService.AreFriendsAsync(userId, friend.Id))
            {
                await Clients.User(friend.Id).SendAsync("ReceiveMessage", messageModel);
                await Clients.Caller.SendAsync("ReceiveMessage", messageModel);
                await _messageService.AddMessageAsync(messageModel);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "You are not friends with this user.");
            }
        }
    }
}
