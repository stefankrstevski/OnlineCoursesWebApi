using OnlineCoursesWebApi.Interfaces.IRepositories;

namespace OnlineCoursesWebApi.Interfaces.IConfiguration
{
    public interface IUnitOfWork
    {
        IApplicationRepository Applications { get; }
        ICompanyRepository Companies { get; }
        IParticipantRepository Participants { get; }
        ICourseDateRepository CourseDates { get; }
        ICourseRepository Courses { get; }
        IApplicationParticipantRepository ApplicationParticipants { get; }


        Task CompleteAsync();
    }
}
