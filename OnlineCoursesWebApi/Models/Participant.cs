namespace OnlineCoursesWebApi.Models
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<ApplicationParticipant> ApplicationParticipants { get; set; } = new List<ApplicationParticipant>();

    }
}
