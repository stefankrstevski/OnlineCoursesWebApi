using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi.Repositories
{
    public class CourseDateRepository : GenericRepository<CourseDate>, ICourseDateRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseDateRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }
        public override async Task<CourseDate> GetById(int id)
        {
            return await _context.CourseDates.Include(x => x.Course).FirstOrDefaultAsync(x => x.CourseDateId == id);
        }
    }
}