namespace OnlineCoursesWebApi.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int CourseDateId { get; set; }
        public CourseDate CourseDate { get; set; } = new CourseDate();
        public int CompanyId { get; set; }
        public Company Company { get; set; } = new Company();
        public ICollection<ApplicationParticipant> ApplicationParticipants { get; set; } = new List<ApplicationParticipant>();
    }
}
