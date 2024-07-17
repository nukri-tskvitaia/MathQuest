using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Entities.Identity;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Services
{
    [TestFixture]
    public class MessageServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IUserService> _userServiceMock;
        private IMessageService _messageService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();

            _messageService = new MessageService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object);
        }

        /*
        [Test]
        public async Task AddMessageAsync_ShouldAddMessage()
        {
            // Arrange
            var model = new MessageModel { SenderId = "sender-id", ReceiverId = "receiver-id", Text = "Test message", SentDate = DateTime.Now };
            var user = new User { Id = "sender-id" };
            _userServiceMock.Setup(m => m.GetUserByIdAsync("receiver-id")).ReturnsAsync(user);
            _userServiceMock.Setup(m => m.GetUserByIdAsync("sender-id")).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<Message>(model)).Returns(new Message());

            // Act
            await _messageService.AddMessageAsync(model);

            // Assert
            _unitOfWorkMock.Verify(u => u.MessageRepository.AddMessageAsync(It.IsAny<Message>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        } */

        [Test]
        public async Task GetAllUsersMessagesAsync_ShouldReturnMessages()
        {
            // Arrange
            var messages = new List<Message> { new Message(), new Message() };
            _unitOfWorkMock
                .Setup(u => u.MessageRepository.GetAllMessagesAsync( 1, 10, 10))
                .ReturnsAsync(new List<IEnumerable<Message>> { messages });
            _mapperMock.Setup(m => m.Map<IEnumerable<MessageModel>>(It.IsAny<IEnumerable<Message>>())).Returns(messages.Select(m => new MessageModel()));

            // Act
            var result = await _messageService.GetAllUsersMessagesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(messages.Count - 1));
        }

        [Test]
        public async Task GetUsersAllMessagesAsync_ShouldReturnMessages()
        {
            // Arrange
            var userId = "user-id";
            var messages = new List<Message> { new Message(), new Message() };
            _unitOfWorkMock
                .Setup(u => u.MessageRepository.GetUsersAllMessagesAsync(
                    userId,
                    1,
                    10,
                    10 
                ))
                .ReturnsAsync(new List<IEnumerable<Message>> { messages });
            _mapperMock.Setup(m => m.Map<IEnumerable<MessageModel>>(It.IsAny<IEnumerable<Message>>())).Returns(messages.Select(m => new MessageModel()));

            // Act
            var result = await _messageService.GetUsersAllMessagesAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(messages.Count - 1));
        }

        [Test]
        public async Task GetUsersMessagesAsync_ShouldReturnMessages()
        {
            // Arrange
            var senderId = "sender-id";
            var receiverId = "receiver-id";
            var messages = new List<Message> { new Message(), new Message() };
            _unitOfWorkMock.Setup(u => u.MessageRepository.GetMessagesByIdsAsync(senderId, receiverId)).ReturnsAsync(messages);
            _mapperMock.Setup(m => m.Map<IEnumerable<MessageModel>>(It.IsAny<IEnumerable<Message>>())).Returns(messages.Select(m => new MessageModel()));

            // Act
            var result = await _messageService.GetUsersMessagesAsync(senderId, receiverId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(messages.Count));
        }

        [Test]
        public async Task GetMessageByIdsAsync_ShouldReturnMessage()
        {
            // Arrange
            var senderId = "sender-id";
            var receiverId = "receiver-id";
            var messageId = 1;
            var message = new Message();
            _unitOfWorkMock.Setup(u => u.MessageRepository.GetMessageByIdsAsync(senderId, receiverId, messageId)).ReturnsAsync(message);
            _mapperMock.Setup(m => m.Map<MessageModel>(It.IsAny<Message>())).Returns(new MessageModel());

            // Act
            var result = await _messageService.GetMessageByIdsAsync(senderId, receiverId, messageId);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /*
        [Test]
        public async Task DeleteMessagesByIdsAsync_ShouldDeleteMessages()
        {
            // Arrange
            var senderId = "sender-id";
            var receiverId = "receiver-id";
            _unitOfWorkMock.Setup(u => u.MessageRepository.DeleteMessagesByIdsAsync(senderId, receiverId)).Returns(Task.CompletedTask);

            // Act
            await _messageService.DeleteMessagesByIdsAsync(senderId, receiverId);

            // Assert
            _unitOfWorkMock.Verify(u => u.MessageRepository.DeleteMessagesByIdsAsync(senderId, receiverId), Times.Once);
        } */

        /*
        [Test]
        public async Task DeleteMessageByIdsAsync_ShouldDeleteMessage()
        {
            // Arrange
            var senderId = "sender-id";
            var receiverId = "receiver-id";
            var messageId = 1;
            _unitOfWorkMock.Setup(u => u.MessageRepository.DeleteMessageByIdsAsync(senderId, receiverId, messageId)).Returns(Task.CompletedTask);

            // Act
            await _messageService.DeleteMessageByIdsAsync(senderId, receiverId, messageId);

            // Assert
            _unitOfWorkMock.Verify(u => u.MessageRepository.DeleteMessageByIdsAsync(senderId, receiverId, messageId), Times.Once);
        } */

        [Test]
        public void Delete_ShouldThrowNotImplementedException()
        {
            // Arrange & Act & Assert
            Assert.Throws<NotImplementedException>(() => _messageService.Delete(new Message()));
        }

        [Test]
        public void Update_ShouldThrowNotImplementedException()
        {
            // Arrange & Act & Assert
            Assert.Throws<NotImplementedException>(() => _messageService.Update(new Message()));
        }
    }
}
