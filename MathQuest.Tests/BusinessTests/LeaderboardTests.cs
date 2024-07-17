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
    public class LeaderboardServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private ILeaderboardService _leaderboardService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _leaderboardService = new LeaderboardService(
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public async Task AddUserPointsAsync_ShouldAddOrUpdatePoints()
        {
            // Arrange
            var model = new LeaderboardDataModel { UserId = "user-id", Points = 100 };
            var existingEntity = new Leaderboard { UserId = "user-id", Points = 50 };
            _unitOfWorkMock.Setup(u => u.LeaderboardRepository.GetByUserIdAsync("user-id")).ReturnsAsync(existingEntity);
            _mapperMock.Setup(m => m.Map<Leaderboard>(model)).Returns(new Leaderboard { UserId = model.UserId, Points = model.Points });

            // Act
            var result = await _leaderboardService.AddUserPointsAsync(model);

            // Assert
            _unitOfWorkMock.Verify(u => u.LeaderboardRepository.Update(It.IsAny<Leaderboard>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.LeaderboardRepository.AddAsync(It.IsAny<Leaderboard>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddUserPointsAsync_ShouldAddNewPoints()
        {
            // Arrange
            var model = new LeaderboardDataModel { UserId = "user-id", Points = 100 };
            _unitOfWorkMock.Setup(u => u.LeaderboardRepository.GetByUserIdAsync("user-id")).ReturnsAsync((Leaderboard)null);
            _mapperMock.Setup(m => m.Map<Leaderboard>(model)).Returns(new Leaderboard { UserId = model.UserId, Points = model.Points });

            // Act
            var result = await _leaderboardService.AddUserPointsAsync(model);

            // Assert
            _unitOfWorkMock.Verify(u => u.LeaderboardRepository.Update(It.IsAny<Leaderboard>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.LeaderboardRepository.AddAsync(It.IsAny<Leaderboard>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddUserPointsAsync_ShouldReturnFalseOnError()
        {
            // Arrange
            var model = new LeaderboardDataModel { UserId = "user-id", Points = 100 };
            _unitOfWorkMock.Setup(u => u.LeaderboardRepository.GetByUserIdAsync("user-id")).ThrowsAsync(new Exception());
            _mapperMock.Setup(m => m.Map<Leaderboard>(model)).Returns(new Leaderboard { UserId = model.UserId, Points = model.Points });

            // Act
            var result = await _leaderboardService.AddUserPointsAsync(model);

            // Assert
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Never);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetBestUserScoresAsync_ShouldReturnScores()
        {
            // Arrange
            var pageSize = 1;
            var count = 20;
            var leaderboardEntities = new List<Leaderboard> { new Leaderboard { UserId = "user1", Points = 100 }, new Leaderboard { UserId = "user2", Points = 90 } };
            _unitOfWorkMock.Setup(u => u.LeaderboardRepository.GetAllWithPaginationAsync(pageSize, count)).ReturnsAsync(leaderboardEntities);
            _mapperMock.Setup(m => m.Map<IEnumerable<LeaderboardDataModel>>(It.IsAny<IEnumerable<Leaderboard>>())).Returns(leaderboardEntities.Select(l => new LeaderboardDataModel { UserId = l.UserId, Points = l.Points }));

            // Act
            var result = await _leaderboardService.GetBestUserScoresAsync(pageSize, count);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(leaderboardEntities.Count));
        }

        [Test]
        public async Task GetUserScoreAsync_ShouldReturnUserScore()
        {
            // Arrange
            var userId = "user-id";
            var leaderboardEntity = new Leaderboard { UserId = userId,Points = 100 };
            _unitOfWorkMock.Setup(u => u.LeaderboardRepository.GetByUserIdAsync(userId)).ReturnsAsync(leaderboardEntity);
            _mapperMock.Setup(m => m.Map<LeaderboardDataModel>(It.IsAny<Leaderboard>())).Returns(new LeaderboardDataModel { UserId = leaderboardEntity.UserId, Points = leaderboardEntity.Points });

            // Act
            var result = await _leaderboardService.GetUserScoreAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Points, Is.EqualTo(leaderboardEntity.Points));
        }
    }
}