using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Helpers;
using OnlineCoursesWebApi.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCoursesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationsService _applicationsService;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(IApplicationsService applicationsService, ILogger<ApplicationsController> logger)
        {
            _applicationsService = applicationsService;
            _logger = logger;
        }

        [HttpPost("save")]
        public async Task<IActionResult> PostApplication(ApplicationDTO applicationDTO)
        {
            try
            {
                await _applicationsService.PostApplication(applicationDTO);
                return Ok(new ApiResponse<bool>(true, "Application submitted successfully."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submitting the application.");
                var errorResponse = new ApiResponse<object>(new List<string> { "An error occurred while submitting the application." }, "Error");
                return StatusCode(500, errorResponse);
            }
        }
    }
}
