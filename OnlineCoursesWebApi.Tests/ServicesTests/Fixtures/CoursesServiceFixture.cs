using academy_web_api.Services;
using OnlineCoursesWebApi.Interfaces.IConfiguration;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Interfaces.IServices;
using Microsoft.Extensions.Logging;
using Moq;

namespace OnlineCoursesWebApi.Tests.ServicesTests.Fixtures
{
    public class CoursesServiceFixture
    {
        public Mock<IUnitOfWork> MockUnitOfWork { get; private set; } = new Mock<IUnitOfWork>();
        public Mock<IAuthService> MockAuthService { get; private set; } = new Mock<IAuthService>();
        public Mock<ICourseRepository> MockCourseRepository { get; private set; } = new Mock<ICourseRepository>();

        public Mock<ICourseApiService> MockCourseApiService { get; private set; } = new Mock<ICourseApiService>();
        public Mock<ILogger<CoursesService>> MockLogger { get; private set; } = new Mock<ILogger<CoursesService>>();
        public CoursesService CoursesService { get; private set; }

        public CoursesServiceFixture()
        {
            CoursesService = new CoursesService(MockUnitOfWork.Object, MockAuthService.Object, MockLogger.Object, MockCourseApiService.Object);
        }
    }

}
