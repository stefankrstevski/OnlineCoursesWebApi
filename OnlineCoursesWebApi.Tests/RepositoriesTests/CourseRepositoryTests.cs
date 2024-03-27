using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Models;
using OnlineCoursesWebApi.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace OnlineCoursesWebApi.Tests.RepositoriesTests
{
    public class CourseRepositoryTests
    {
        private readonly Mock<ILogger<CourseRepository>> _mockLogger = new Mock<ILogger<CourseRepository>>();
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public CourseRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllActiveCoursesAsync_ReturnsActiveCourses()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                var courses = new List<Course>
                {
                    new Course { Name = "Course 1", Dates = new List<CourseDate> { new CourseDate { Date = DateTime.Today } } },
                };

                context.AddRange(courses);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var repository = new CourseRepository(context, _mockLogger.Object);

                // Act
                var result = await repository.GetAllActiveCoursesAsync();

                // Assert
                result.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task SyncCoursesAsync_AddsNewCoursesAndUpdatesExistingOnes()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                context.Courses.Add(new Course { Name = "Existing Course", Dates = new List<CourseDate> { new CourseDate { Date = DateTime.Today.AddDays(-1) } } });
                await context.SaveChangesAsync();
            }

            var newCourses = new List<Course>
            {
                new Course { Name = "New Course", Dates = new List<CourseDate> { new CourseDate { Date = DateTime.Today } } },
                new Course { Name = "Existing Course", Dates = new List<CourseDate> { new CourseDate { Date = DateTime.Today } } }
            };

            using (var context = new ApplicationDbContext(_options))
            {
                var repository = new CourseRepository(context, _mockLogger.Object);

                // Act
                await repository.SyncCoursesAsync(newCourses);
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var allCourses = await context.Courses.Include(c => c.Dates).ToListAsync();

                // Assert
                allCourses.Should().HaveCount(2, "one new course should be added and one existing course should be updated");

                var updatedCourse = allCourses.FirstOrDefault(c => c.Name == "Existing Course");
                updatedCourse.Should().NotBeNull();
                updatedCourse.Dates.Should().ContainSingle(d => d.Date == DateTime.Today, "the existing course should be updated with a new date");

                var addedCourse = allCourses.FirstOrDefault(c => c.Name == "New Course");
                addedCourse.Should().NotBeNull();
                addedCourse.Dates.Should().ContainSingle(d => d.Date == DateTime.Today, "the new course should be added");
            }
        }
    }
}
