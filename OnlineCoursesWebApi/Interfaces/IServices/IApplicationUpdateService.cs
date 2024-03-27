using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Interfaces.IServices
{
    public interface IApplicationUpdateService
    {
        Task UpdateApplicationParticipants(Application application, ApplicationDTO applicationDto);

    }
}
