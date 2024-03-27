using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Interfaces.IConfiguration;
using OnlineCoursesWebApi.Interfaces.IServices;
using OnlineCoursesWebApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace academy_web_api.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ICourseApiService _courseApiService;
        private readonly ILogger<CoursesService> _logger;

        public CoursesService(IUnitOfWork unitOfWork, IAuthService authService, ILogger<CoursesService> logger, ICourseApiService courseApiService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _logger = logger;
            _courseApiService = courseApiService;
        }

        public async Task<IEnumerable<ClientCourseDTO>> GetAllActiveCoursesAsync()
        {
            try
            {
                var courses = await _unitOfWork.Courses.GetAllActiveCoursesAsync();
                var courseDTOs = courses.Select(course => new ClientCourseDTO
                {
                    CourseId = course.CourseId,
                    Name = course.Name,
                    Dates = course.Dates.Select(date => date.Date).ToList()
                });

                return courseDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all active courses from the repository.");
                throw;
            }
        }
        public async Task SyncCoursesWithExternalApiAsync()
        {
            try
            {
                var token = await _authService.GetAuthTokenAsync();
                var coursesFromApi = await _courseApiService.FetchAllCoursesAsync(token);
                var activeCourses = TransformCourses(coursesFromApi);
                if(activeCourses.IsNullOrEmpty())
                {
                    _logger.LogWarning("No active Courses");
                    return;                    
                }
                await _unitOfWork.Courses.SyncCoursesAsync(activeCourses);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync courses with the external API.");
                throw;
            }
        }

        private IEnumerable<Course> TransformCourses(IEnumerable<ExternalCourseDTO> coursesFromApi)
        {
            var groupedCourses = coursesFromApi
                    .GroupBy(c => c.Id)
                    .Select(g => new Course
                    {
                        Name = g.First().CourseName,
                        Dates = g.Where(d => d.IsActive).Select(d => new CourseDate { Date = d.Date }).ToList()
                    })
                    .Where(c => c.Dates.Any())
                    .ToList();

            return groupedCourses;
        }

    }
}