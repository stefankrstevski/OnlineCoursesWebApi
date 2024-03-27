using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Interfaces.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<Company> FindByEmailAsync(string email);
    }
}
