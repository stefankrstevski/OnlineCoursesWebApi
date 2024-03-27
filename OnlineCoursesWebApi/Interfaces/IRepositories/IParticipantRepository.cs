using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Interfaces.IRepositories
{
    public interface IParticipantRepository:IGenericRepository<Participant>
    {
        Task<Participant> FindByEmailAsync(string email);
    }
}
