using OnlineCoursesWebApi.Interfaces.IConfiguration;
using Microsoft.Extensions.Logging;
using Moq;

namespace OnlineCoursesWebApi.Tests.ServicesTests.Fixtures
{
    public class ApplicationServiceFixture
    {
        public Mock<IUnitOfWork> MockUnitOfWork { get; private set; }
        public Mock<ILogger<ApplicationsService>> MockLogger { get; private set; }
        public ApplicationsService Service { get; private set; }

        public ApplicationServiceFixture()
        {
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockLogger = new Mock<ILogger<ApplicationsService>>();
            Service = new ApplicationsService(MockUnitOfWork.Object, MockLogger.Object);
        }
    }
}
