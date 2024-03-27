namespace OnlineCoursesWebApi.DTOs
{
    public class ApplicationDTO
    {
        public int CourseDateId { get; set; }
        public CompanyDTO CompanyDTO { get; set; } = new CompanyDTO();
        public List<ParticipantDTO> ParticipantDTOs { get; set; } = new List<ParticipantDTO>();
    }
}
