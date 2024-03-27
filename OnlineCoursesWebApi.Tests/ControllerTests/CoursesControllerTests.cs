using OnlineCoursesWebApi.Controllers;
using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Helpers;
using OnlineCoursesWebApi.Interfaces.IServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace OnlineCoursesWebApi.Tests.ControllerTests
{
    public class CoursesControllerTests
    {
        private readonly Mock<ICoursesService> _mockCoursesService = new Mock<ICoursesService>();
        private readonly Mock<ILogger<CoursesController>> _mockLogger = new Mock<ILogger<CoursesController>>();
        private readonly CoursesController _controller;

        public CoursesControllerTests()
        {
            _controller = new CoursesController(_mockCoursesService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllActiveCourses_ReturnsCourses()
        {
            // Arrange
            var courses = new List<ClientCourseDTO>
        {
            new ClientCourseDTO { CourseId = 1, Name = "Test Course", Dates = new List<DateTime>() { DateTime.Today } }
            // Add more courses as needed
        };

            _mockCoursesService.Setup(service => service.GetAllActiveCoursesAsync()).ReturnsAsync(courses);

            // Act
            var result = await _controller.GetAllActiveCourses();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ClientCourseDTO>>>().Subject;
            apiResponse.Data.Should().HaveCount(courses.Count);
            apiResponse.Message.Should().Be("Active courses retrieved successfully.");
        }

        [Fact]
        public async Task GetAllActiveCourses_ReturnsNoCoursesFound()
        {
            // Arrange
            _mockCoursesService.Setup(service => service.GetAllActiveCoursesAsync()).ReturnsAsync(new List<ClientCourseDTO>());

            // Act
            var result = await _controller.GetAllActiveCourses();

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var apiResponse = notFoundResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ClientCourseDTO>>>().Subject;
            apiResponse.Data.Should().BeEmpty();
            apiResponse.Message.Should().Be("No active courses found.");
        }

        [Fact]
        public async Task GetAllActiveCourses_ReturnsException()
        {
            // Arrange
            _mockCoursesService.Setup(service => service.GetAllActiveCoursesAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllActiveCourses();

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(500);
            var apiResponse = statusCodeResult.Value.Should().BeAssignableTo<ApiResponse<IEnumerable<ClientCourseDTO>>>().Subject;
            apiResponse.Data.Should().BeEmpty();
            apiResponse.Message.Should().Contain("unexpected error occurred");
        }
    }
}
