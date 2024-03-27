using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<Company> FindByEmailAsync(string email)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
