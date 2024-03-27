using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Interfaces.IRepositories
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<Application> FindByCourseDateAndCompany(int courseDateId, int companyId);
    }
}
