
using academy_web_api.Services;
using OnlineCoursesWebApi.Configuration;
using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IConfiguration;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Interfaces.IServices;
using OnlineCoursesWebApi.Repositories;
using OnlineCoursesWebApi.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Configure external authentication

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
            builder.Services.AddHttpClient<IAuthService, AuthService>();
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddHostedService<CoursesSyncBackgroundService>();
            builder.Services.AddScoped<ICoursesService, CoursesService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICourseApiService, CourseApiService>();

            builder.Services.AddScoped<IApplicationsService, ApplicationsService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registering DbContext
            var sqlConnection = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(sqlConnection ??
                    throw new InvalidOperationException("Connection String is not found"));
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }
            //use swagger despite the environment
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
