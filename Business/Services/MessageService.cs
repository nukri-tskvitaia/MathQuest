using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public MessageService(IUnitOfWork context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task AddMessageAsync(MessageModel model)
        {
            if (model == null)
            {
                throw new MathQuestException("Message can't be empty.");
            }

            _ = await _userService.GetUserByIdAsync(model.ReceiverId)
                ?? throw new MathQuestException("One of the users don't exist.");
            _ = await _userService.GetUserByIdAsync(model.SenderId)
                ?? throw new MathQuestException("One of the users don't exist.");

            var message = _mapper.Map<Message>(model);
            
            await _context.MessageRepository.AddMessageAsync(message);
            await _context.SaveAsync();
        }

        public void Delete(Message entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessageByIdsAsync(string senderId, string receiverId, int messageId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessagesByIdsAsync(string senderId, string receiverId)
        {
            throw new NotImplementedException();
        }

        // this is for admins
        public async Task<IEnumerable<IEnumerable<MessageModel>>> GetAllUsersMessagesAsync(int pageNumber = 1, int pageSize = 10, int limitGroups = 10)
        {
            var messages = await _context.MessageRepository.GetAllMessagesAsync();
            List<IEnumerable<MessageModel>> model = [];

            foreach (var message in messages)
            {
                var mappedMessages = message.Select(x => _mapper.Map<MessageModel>(x));
                model.Add(mappedMessages);
            }

            return model;
        }

        public async Task<IEnumerable<IEnumerable<MessageModel>>> GetUsersAllMessagesAsync(string userId, int pageNumber = 1, int pageSize = 10, int limitGroups = 10)
        {
            var messages = await _context.MessageRepository.GetUsersAllMessagesAsync(userId);
            List<IEnumerable<MessageModel>> model = [];

            foreach (var message in messages)
            {
                var mappedMessages = message.Select(x => _mapper.Map<MessageModel>(x));
                model.Add(mappedMessages);
            }

            return model;
        }

        public async Task<IEnumerable<MessageModel>> GetUsersMessagesAsync(string senderId, string receiverId)
        {
            var messages = await _context.MessageRepository.GetMessagesByIdsAsync(senderId, receiverId);
            List<MessageModel> model = [];

            foreach (var message in messages)
            {
                model.Add(_mapper.Map<MessageModel>(message));
            }

            return model;
        }

        public async Task<MessageModel> GetMessageByIdsAsync(string senderId, string receiverId, int messageId)
        {
            var message = await _context.MessageRepository.GetMessageByIdsAsync(senderId, receiverId, messageId) 
                ?? throw new MathQuestException("No such message found.");
            
            var messageModel = _mapper.Map<MessageModel>(message);
            return messageModel;

        }

        public void Update(Message entity)
        {
            throw new NotImplementedException();
        }
    }
}
