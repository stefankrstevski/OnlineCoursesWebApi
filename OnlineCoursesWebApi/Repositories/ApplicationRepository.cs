using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<Application> FindByCourseDateAndCompany(int courseDateId, int companyId)
        {
            return await _context.Applications.Include(x => x.ApplicationParticipants).ThenInclude(ap => ap.Participant).FirstOrDefaultAsync(x => x.CourseDateId == courseDateId && x.CompanyId == companyId);
        }
    }
}
