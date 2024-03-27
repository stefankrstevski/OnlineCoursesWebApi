using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public CourseRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Course>> GetAllActiveCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Dates)
                .Where(c => c.Dates.Any())
                .ToListAsync();
        }

        public async Task SyncCoursesAsync(IEnumerable<Course> newCourses)
        {
            try
            {
                foreach (var newCourse in newCourses)
                {
                    var existingCourse = await _context.Courses
                        .Include(c => c.Dates)
                        .FirstOrDefaultAsync(c => c.Name == newCourse.Name);

                    if (existingCourse == null)
                    {
                        _context.Courses.Add(newCourse);
                    }
                    else
                    {
                        existingCourse.Name = newCourse.Name;

                        foreach (var newDate in newCourse.Dates)
                        {
                            var existingDate = existingCourse.Dates
                                .FirstOrDefault(d => d.Date == newDate.Date);

                            if (existingDate == null)
                            {
                                existingCourse.Dates.Add(newDate);
                            }
                        }

                        existingCourse.Dates = existingCourse.Dates
                            .Where(d => newCourse.Dates.Any(nd => nd.Date == d.Date))
                            .ToList();
                    }
                }


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing courses with the database.");
                throw;
            }


        }
    }
}
