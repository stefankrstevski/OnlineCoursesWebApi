using OnlineCoursesWebApi.Interfaces.IServices;

namespace OnlineCoursesWebApi.Services
{
    public class CoursesSyncBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CoursesSyncBackgroundService> _logger;

        public CoursesSyncBackgroundService(IServiceProvider serviceProvider, ILogger<CoursesSyncBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Courses Sync Background Service is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Courses data sync is starting.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var coursesService = scope.ServiceProvider.GetRequiredService<ICoursesService>();
                    try
                    {
                        await coursesService.SyncCoursesWithExternalApiAsync();
                        _logger.LogInformation("Courses data synced successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred executing sync courses data.");
                    }
                }

                // Sync once every day
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
