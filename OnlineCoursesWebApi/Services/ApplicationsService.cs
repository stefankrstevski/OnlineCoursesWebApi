using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Interfaces.IConfiguration;
using OnlineCoursesWebApi.Interfaces.IServices;
using OnlineCoursesWebApi.Models;

public class ApplicationsService : IApplicationsService
{
    private readonly ILogger<ApplicationsService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationsService(IUnitOfWork unitOfWork, ILogger<ApplicationsService> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task PostApplication(ApplicationDTO applicationDto)
    {
        try
        {
            Application application = await InitializeApplication(applicationDto);
            await UpdateApplicationParticipants(application, applicationDto);
            await _unitOfWork.CompleteAsync(); // Save changes including new participants and application 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to post application.");
            throw;
        }
    }

    #region HelperMethods
    public async Task<Application> InitializeApplication(ApplicationDTO applicationDto)
    {
        var courseDate = await _unitOfWork.CourseDates.GetById(applicationDto.CourseDateId)
                         ?? throw new ArgumentException("Invalid CourseDateId.");

        var company = await GetOrCreateCompany(applicationDto.CompanyDTO);

        var application = await _unitOfWork.Applications.FindByCourseDateAndCompany(courseDate.CourseDateId, company.CompanyId)
                         ?? await CreateApplication(courseDate, company);

        return application;
    }

    public async Task<Company> GetOrCreateCompany(CompanyDTO companyDto)
    {
        var existingCompany = await _unitOfWork.Companies.FindByEmailAsync(companyDto.Email);
        if (existingCompany == null)
        {
            existingCompany = new Company
            {
                Name = companyDto.Name,
                PhoneNumber = companyDto.PhoneNumber,
                Email = companyDto.Email
            };
            _unitOfWork.Companies.Add(existingCompany);
        }
        return existingCompany;
    }

    public async Task<Application> CreateApplication(CourseDate courseDate, Company company)
    {
        var application = new Application
        {
            CourseDateId = courseDate.CourseDateId,
            CourseDate = courseDate,
            CompanyId = company.CompanyId,
            Company = company,
        };
        _unitOfWork.Applications.Add(application);

        return application;
    }

    public async Task UpdateApplicationParticipants(Application application, ApplicationDTO applicationDto)
    {
        var currentParticipantEmails = application.ApplicationParticipants
            .Select(ap => ap.Participant.Email)
            .ToList();
        var updatedParticipantEmails = applicationDto.ParticipantDTOs.Select(p => p.Email).ToList();
        var participantsToRemove = application.ApplicationParticipants
            .Where(ap => !updatedParticipantEmails.Contains(ap.Participant.Email))
            .ToList();

        foreach (var applicationParticipant in participantsToRemove)
        {
            application.ApplicationParticipants.Remove(applicationParticipant);
            _unitOfWork.ApplicationParticipants.Remove(applicationParticipant);
        }

        // Process new and existing participants
        foreach (var participantDTO in applicationDto.ParticipantDTOs)
        {
            // participant exists skip to avoid duplicate
            if (currentParticipantEmails.Contains(participantDTO.Email))
            {
                continue;
            }

            var existingParticipant = await _unitOfWork.Participants.FindByEmailAsync(participantDTO.Email);

            if (existingParticipant == null)
            {
                // create new participant
                existingParticipant = new Participant
                {
                    Name = participantDTO.Name,
                    PhoneNumber = participantDTO.PhoneNumber,
                    Email = participantDTO.Email
                };
                // add to database
                _unitOfWork.Participants.Add(existingParticipant);
            }

            var applicationParticipant = new ApplicationParticipant
            {
                Application = application,
                Participant = existingParticipant
            };

            application.ApplicationParticipants.Add(applicationParticipant);
        }
    }
    #endregion


}
