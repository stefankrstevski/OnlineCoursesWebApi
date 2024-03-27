using OnlineCoursesWebApi.Data;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesWebApi.Repositories
{
    public class ParticipantRepository : GenericRepository<Participant>, IParticipantRepository
    {
        private readonly ApplicationDbContext _context;
        public ParticipantRepository(
            ApplicationDbContext context,
            ILogger logger
            ) : base(context, logger)
        {
            _context = context;
        }

        public async Task<Participant> FindByEmailAsync(string email)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);
        }
    }
}
