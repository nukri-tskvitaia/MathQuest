using System.Threading.Tasks;
using Business.Interfaces;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Services
{
    [TestFixture]
    public class GradeServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private IGradeService _gradeService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gradeService = new GradeService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetGradeByIdAsync_ShouldReturnGrade()
        {
            // Arrange
            var gradeId = 1;
            var expectedGrade = new Grade { Id = gradeId, GradeName = "Grade 1", GradeLevel = 1 };

            _unitOfWorkMock.Setup(u => u.GradeRepository.GetByIdAsync(gradeId)).ReturnsAsync(expectedGrade);

            // Act
            var result = await _gradeService.GetGradeByIdAsync(gradeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedGrade.Id));
            Assert.That(result.GradeName, Is.EqualTo(expectedGrade.GradeName));
            Assert.That(result.GradeLevel, Is.EqualTo(expectedGrade.GradeLevel));
        }

        [Test]
        public async Task GetGradeLevelByIdAsync_ShouldReturnGradeLevel()
        {
            // Arrange
            var gradeId = 1;
            var expectedGrade = new Grade { Id = gradeId, GradeName = "Grade 1", GradeLevel = 1 };

            _unitOfWorkMock.Setup(u => u.GradeRepository.GetByIdAsync(gradeId)).ReturnsAsync(expectedGrade);

            // Act
            var result = await _gradeService.GetGradeLevelByIdAsync(gradeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedGrade.GradeLevel));
        }

        [Test]
        public async Task GetGradeNameByIdAsync_ShouldReturnGradeName()
        {
            // Arrange
            var gradeId = 1;
            var expectedGrade = new Grade { Id = gradeId, GradeName = "Grade 1", GradeLevel = 1 };

            _unitOfWorkMock.Setup(u => u.GradeRepository.GetByIdAsync(gradeId)).ReturnsAsync(expectedGrade);

            // Act
            var result = await _gradeService.GetGradeNameByIdAsync(gradeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedGrade.GradeName));
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWorkMock = null;
            _gradeService = null;
        }
    }
}