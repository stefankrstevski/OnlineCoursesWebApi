using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Interfaces.IRepositories
{
    public interface IApplicationParticipantRepository : IGenericRepository<ApplicationParticipant>
    {
        Task<bool> Remove(ApplicationParticipant applicationParticipant);
    }
}
