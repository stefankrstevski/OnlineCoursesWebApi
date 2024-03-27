using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;

namespace OnlineCoursesWebApi.Repositories
{
    public class ApplicationParticipantRepository : GenericRepository<ApplicationParticipant>, IApplicationParticipantRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationParticipantRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<bool> Remove(ApplicationParticipant applicationParticipant)
        {
            try
            {
                if (applicationParticipant != null)
                {
                    _context.Remove(applicationParticipant);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Model empty");
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting ApplicationParticipant with applicationId {applicationParticipant.ApplicationId} and participantId {applicationParticipant.ParticipantId}", applicationParticipant.ApplicationId, applicationParticipant.ParticipantId);
                return false;
            }
        }
    }
}
