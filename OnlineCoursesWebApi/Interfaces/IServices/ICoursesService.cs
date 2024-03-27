using OnlineCoursesWebApi.DTOs;

namespace OnlineCoursesWebApi.Interfaces.IServices
{
    public interface ICoursesService
    {
        Task SyncCoursesWithExternalApiAsync();
        Task<IEnumerable<ClientCourseDTO>> GetAllActiveCoursesAsync();
    }
}