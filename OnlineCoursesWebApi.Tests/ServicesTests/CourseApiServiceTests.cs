using OnlineCoursesWebApi.Configuration;
using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Services;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using System.Net;
using System.Text.Json;

namespace OnlineCoursesWebApi.Tests.ServicesTests
{
    public class CourseApiServiceTests
    {
        [Fact]
        public async Task FetchAllCoursesAsync_ReturnsCourses()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var coursesApiResponse = new CoursesApiResponse
            {
                Data = new List<ExternalCourseDTO>
            {
                new ExternalCourseDTO { Id = 1, CourseName = "Test Course 1", IsActive = true },
                new ExternalCourseDTO { Id = 2, CourseName = "Test Course 2", IsActive = true }
            }
            };
            var apiResponseJson = JsonSerializer.Serialize(coursesApiResponse);
            SetupMockHttpMessageHandler(mockHttpMessageHandler, apiResponseJson, HttpStatusCode.OK);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("http://test.com/")
            };

            var mockOptions = new Mock<IOptions<ApiSettings>>();
            mockOptions.Setup(ap => ap.Value).Returns(new ApiSettings { ApiUrl = "http://test.com/api" });

            var service = new CourseApiService(httpClient, mockOptions.Object);

            // Act
            var result = await service.FetchAllCoursesAsync("dummy_token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<ExternalCourseDTO>)result).Count);
        }

        [Fact]
        public async Task FetchAllCoursesAsync_ThrowsInvalidException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            SetupMockHttpMessageHandler(mockHttpMessageHandler, "", HttpStatusCode.BadRequest); // Simulate a bad request response

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("http://test.com/")
            };

            var mockOptions = new Mock<IOptions<ApiSettings>>();
            mockOptions.Setup(ap => ap.Value).Returns(new ApiSettings { ApiUrl = "http://test.com/api" });

            var service = new CourseApiService(httpClient, mockOptions.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.FetchAllCoursesAsync("dummy_token"));
        }

        private void SetupMockHttpMessageHandler(Mock<HttpMessageHandler> mockHttpMessageHandler, string responseBody, HttpStatusCode statusCode)
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseBody)
                })
                .Verifiable();
        }
    }
}
