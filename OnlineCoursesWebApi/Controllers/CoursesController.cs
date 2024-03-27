using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Helpers;
using OnlineCoursesWebApi.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCoursesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _coursesService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICoursesService coursesService, ILogger<CoursesController> logger)
        {
            _coursesService = coursesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveCourses()
        {
            try
            {
                var courses = await _coursesService.GetAllActiveCoursesAsync();
                if (!courses.Any())
                {
                    return NotFound(new ApiResponse<IEnumerable<ClientCourseDTO>>(courses, "No active courses found."));
                }
                return Ok(new ApiResponse<IEnumerable<ClientCourseDTO>>(courses, "Active courses retrieved successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all active courses.");
                var errorResponse = new ApiResponse<IEnumerable<ClientCourseDTO>>(new List<ClientCourseDTO>(), "An unexpected error occurred while retrieving courses. Please try again later.");
                return StatusCode(500, errorResponse);
            }
        }
    }
}