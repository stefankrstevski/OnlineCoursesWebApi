using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Interfaces.IRepositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetAllActiveCoursesAsync();
        Task SyncCoursesAsync(IEnumerable<Course> courses);
    }
}
