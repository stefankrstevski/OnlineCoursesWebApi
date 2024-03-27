using OnlineCoursesWebApi.Interfaces.IConfiguration;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Repositories;

namespace OnlineCoursesWebApi.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public IApplicationRepository Applications { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IParticipantRepository Participants { get; private set; }
        public ICourseDateRepository CourseDates { get; private set; }
        public ICourseRepository Courses { get; private set; }
        public IApplicationParticipantRepository ApplicationParticipants { get; private set; }

        public UnitOfWork(
            ApplicationDbContext context,
            ILoggerFactory loggerFactory
            )
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<UnitOfWork>();

            Applications = new ApplicationRepository(context, _logger);
            Companies = new CompanyRepository(context, _logger);
            Participants = new ParticipantRepository(context, _logger);
            CourseDates = new CourseDateRepository(context, _logger);
            Courses = new CourseRepository(context, _logger);
            ApplicationParticipants = new ApplicationParticipantRepository(context, _logger);
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
