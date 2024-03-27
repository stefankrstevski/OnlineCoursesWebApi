namespace OnlineCoursesWebApi.Models
{
    public class ApplicationParticipant
    {
        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
    }
}
