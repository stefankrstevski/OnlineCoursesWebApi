using OnlineCoursesWebApi.DTOs;

namespace OnlineCoursesWebApi.Interfaces.IConfiguration
{
    public interface ICourseApiService
    {
        Task<IEnumerable<ExternalCourseDTO>> FetchAllCoursesAsync(string token);
    }
}
