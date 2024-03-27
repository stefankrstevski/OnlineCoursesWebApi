using OnlineCoursesWebApi.DTOs;
namespace OnlineCoursesWebApi.Interfaces.IServices
{
    public interface IApplicationsService
    {
        Task PostApplication(ApplicationDTO applicationDto);
    }
}
