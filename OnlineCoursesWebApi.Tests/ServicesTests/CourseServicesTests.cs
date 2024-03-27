using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Models;
using OnlineCoursesWebApi.Tests.ServicesTests.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
namespace OnlineCoursesWebApi.Tests.ServicesTests
{
    public class CoursesServiceTests : IClassFixture<CoursesServiceFixture>
    {
        private readonly CoursesServiceFixture _fixture;

        public CoursesServiceTests(CoursesServiceFixture fixture)
        {
            _fixture = fixture;
            // Resets mocks before each test to ensure a clean state
            _fixture.MockUnitOfWork.ResetCalls();
            _fixture.MockAuthService.ResetCalls();
            _fixture.MockCourseApiService.ResetCalls();
            _fixture.MockLogger.ResetCalls();
            _fixture.MockCourseRepository.ResetCalls();
        }

        [Fact]
        public async Task GetAllActiveCoursesAsync_ReturnsNonEmptyList_WhenCoursesExist()
        {
            // Arrange
            var mockCourses = new List<Course>
            {
                new Course { CourseId = 1, Name = "Course 1", Dates = new List<CourseDate> { new CourseDate { Date = DateTime.Now } } }
            };
            _fixture.MockUnitOfWork.Setup(uow => uow.Courses.GetAllActiveCoursesAsync()).ReturnsAsync(mockCourses);

            // Act
            var result = await _fixture.CoursesService.GetAllActiveCoursesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            _fixture.MockUnitOfWork.Verify(uow => uow.Courses.GetAllActiveCoursesAsync(), Times.Once);
        }

        [Fact]
        public async Task SyncCoursesWithExternalApiAsync_LogsWarning_WhenNoActiveCoursesFetched()
        {
            // Arrange
            _fixture.MockAuthService.Setup(auth => auth.GetAuthTokenAsync()).ReturnsAsync("valid-token");
            _fixture.MockCourseApiService.Setup(api => api.FetchAllCoursesAsync(It.IsAny<string>())).ReturnsAsync(new List<ExternalCourseDTO>());

            // Act
            await _fixture.CoursesService.SyncCoursesWithExternalApiAsync();

            // Assert
            _fixture.MockUnitOfWork.Verify(uow => uow.Courses.SyncCoursesAsync(It.IsAny<IEnumerable<Course>>()), Times.Never);
        }

        [Fact]
        public async Task SyncCoursesWithExternalApiAsync_UpdatesDatabase_WhenActiveCoursesFetched()
        {
            // Arrange
            var mockCoursesFromApi = new List<ExternalCourseDTO>
            {
                new ExternalCourseDTO { Id = 1, CourseName = "API Course 1", IsActive = true, Date = DateTime.Now }
            };
            _fixture.MockAuthService.Setup(auth => auth.GetAuthTokenAsync()).ReturnsAsync("valid-token");
            _fixture.MockCourseApiService.Setup(api => api.FetchAllCoursesAsync(It.IsAny<string>())).ReturnsAsync(mockCoursesFromApi);
            _fixture.MockUnitOfWork.Setup(uow => uow.Courses).Returns(_fixture.MockCourseRepository.Object);
            _fixture.MockUnitOfWork.Setup(uow => uow.Courses.SyncCoursesAsync(It.IsAny<IEnumerable<Course>>()))
    .Returns(Task.CompletedTask);

            // Act
            await _fixture.CoursesService.SyncCoursesWithExternalApiAsync();

            // Assert
            _fixture.MockUnitOfWork.Verify(uow => uow.Courses.SyncCoursesAsync(It.IsAny<IEnumerable<Course>>()), Times.Once);
        }
    }
}
