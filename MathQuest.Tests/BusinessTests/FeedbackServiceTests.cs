using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Services
{
    [TestFixture]
    public class FeedbackServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private FeedbackService _feedbackService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _feedbackService = new FeedbackService(
                _mapperMock.Object,
                _unitOfWorkMock.Object);
        }

        [Test]
        public async Task AddFeedbackAsync_ShouldAddFeedback()
        {
            // Arrange
            var feedbackModel = new FeedbackModel { Id = 1, UserId = "user-id", FeedbackText = "Test feedback" };
            var feedback = new Feedback { Id = feedbackModel.Id, UserId = feedbackModel.UserId, FeedbackText = feedbackModel.FeedbackText };

            _mapperMock.Setup(x => x.Map<Feedback>(feedbackModel)).Returns(feedback).Verifiable();
            _unitOfWorkMock.Setup(x => x.FeedbackRepository.AddAsync(feedback)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _feedbackService.AddFeedbackAsync(feedbackModel);

            // Assert
            _mapperMock.Verify();
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task DeleteFeedbackByUserIdAsync_ShouldDeleteFeedback()
        {
            // Arrange
            var userId = "user-id";
            var feedbackId = 1;
            var feedback = new Feedback { Id = feedbackId, UserId = userId };

            _unitOfWorkMock.Setup(x => x.FeedbackRepository.GetByUserAndFeedbackIdsAsync(userId, feedbackId)).ReturnsAsync(feedback).Verifiable();
            _unitOfWorkMock.Setup(x => x.FeedbackRepository.Delete(feedback)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _feedbackService.DeleteFeedbackByUserIdAsync(userId, feedbackId);

            // Assert
            _unitOfWorkMock.Verify();
        }

        /*
        [Test]
        public async Task DeleteUserFeedbacksInPeriodAsync_ShouldDeleteFeedbacksInRange()
        {
            // Arrange
            var userId = "user-id";
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);
            var feedbacks = new List<Feedback>
            {
                new Feedback { UserId = userId, FeedbackDate = new DateTime(2023, 3, 15) },
                new Feedback { UserId = userId, FeedbackDate = new DateTime(2023, 6, 20) }
            };

            _unitOfWorkMock.Setup(x => x.FeedbackRepository.GetUserFeedbacksByUserIdAsync(userId, fromDate, toDate)).ReturnsAsync(feedbacks).Verifiable();
            _unitOfWorkMock.Setup(x => x.FeedbackRepository.DeleteRange(It.IsAny<IEnumerable<Feedback>>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _feedbackService.DeleteUserFeedbacksInPeriodAsync(userId, fromDate, toDate);

            // Assert
            _unitOfWorkMock.Verify();
        } */

        [Test]
        public async Task GetAllFeedbacksAsync_ShouldReturnFilteredFeedbacks()
        {
            // Arrange
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);
            var feedbacks = new List<Feedback>
    {
        new Feedback { Id = 1, FeedbackDate = new DateTime(2023, 3, 15) },
        new Feedback { Id = 2, FeedbackDate = new DateTime(2023, 6, 20) }
    };

            _unitOfWorkMock.Setup(x => x.FeedbackRepository.GetAllAsync()).ReturnsAsync(feedbacks).Verifiable();

            // Act
            var result = await _feedbackService.GetAllFeedbacksAsync(fromDate, toDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task GetUsersAllFeedbacksByUserIdAsync_ShouldReturnFilteredFeedbacks()
        {
            // Arrange
            var userId = "user-id";
            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);
            var userFeedbacks = new List<Feedback>
    {
        new Feedback { UserId = userId, FeedbackDate = new DateTime(2023, 3, 15) },
        new Feedback { UserId = userId, FeedbackDate = new DateTime(2023, 6, 20) }
    };

            _unitOfWorkMock.Setup(x => x.FeedbackRepository.GetUserFeedbacksByUserIdAsync(userId, fromDate, toDate)).ReturnsAsync(userFeedbacks).Verifiable();

            // Act
            var result = await _feedbackService.GetUsersAllFeedbacksByUserIdAsync(userId, fromDate, toDate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
            _unitOfWorkMock.Verify();
        }

        /*
        [Test]
        public async Task GetSingleUserFeedbackAsync_ShouldReturnFeedback()
        {
            // Arrange
            var userId = "user-id";
            var feedbackId = 1;
            var feedback = new Feedback { Id = feedbackId, UserId = userId };

            _unitOfWorkMock.Setup(x => x.FeedbackRepository.GetByUserAndFeedbackIdsAsync(userId, feedbackId)).ReturnsAsync(feedback).Verifiable();

            // Act
            var result = await _feedbackService.GetSingleUserFeedbackAsync(userId, feedbackId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(feedbackId));
            _unitOfWorkMock.Verify();
        } */

        private IEnumerable<Feedback> GetTestFeedbacks()
        {
            return new List<Feedback>
            {
                new Feedback { Id = 1, UserId = "user-1", FeedbackText = "Test Feedback 1", FeedbackDate = new DateTime(2023, 3, 15) },
                new Feedback { Id = 2, UserId = "user-1", FeedbackText = "Test Feedback 2", FeedbackDate = new DateTime(2023, 6, 20) },
                new Feedback { Id = 3, UserId = "user-2", FeedbackText = "Test Feedback 3", FeedbackDate = new DateTime(2023, 5, 10) }
            };
        }
    }
}