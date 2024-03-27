using OnlineCoursesWebApi.DTOs;
using OnlineCoursesWebApi.Interfaces.IRepositories;
using OnlineCoursesWebApi.Models;
using OnlineCoursesWebApi.Tests.ServicesTests.Fixtures;
using Moq;
using FluentAssertions;

namespace OnlineCoursesWebApi.Tests.ServicesTests
{
    public class ApplicationServiceTests : IClassFixture<ApplicationServiceFixture>
    {
        private readonly ApplicationServiceFixture _fixture;
        public ApplicationServiceTests(
            ApplicationServiceFixture fixture
            )
        {
            _fixture = fixture;
            _fixture.MockUnitOfWork.Reset();
            _fixture.MockLogger.Reset();
        }
        [Fact]
        public async Task InitializeApplication_ShouldReturnExistingApplication()
        {
            // Arrange
            var applicationDto = new ApplicationDTO { CourseDateId = 1, CompanyDTO = new CompanyDTO { Email = "test@text.com" } };
            var expectedApplication = new Application();

            _fixture.MockUnitOfWork.Setup(u => u.CourseDates.GetById(applicationDto.CourseDateId)).ReturnsAsync(new CourseDate());
            _fixture.MockUnitOfWork.Setup(u => u.Companies.FindByEmailAsync(applicationDto.CompanyDTO.Email)).ReturnsAsync(new Company());
            _fixture.MockUnitOfWork.Setup(u => u.Applications.FindByCourseDateAndCompany(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedApplication);

            // Act
            var application = await _fixture.Service.InitializeApplication(applicationDto);

            // Assert
            application.Should().NotBeNull();
            application.Should().Be(expectedApplication);
        }

        [Fact]
        public async Task InitializeApplication_CourseDateDoesNotExist()
        {
            // Arrange
            var applicationDto = new ApplicationDTO { CourseDateId = 99, CompanyDTO = new CompanyDTO { Email = "test@company.com" } };

            _fixture.MockUnitOfWork.Setup(u => u.CourseDates.GetById(applicationDto.CourseDateId)).ReturnsAsync(value: null);

            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fixture.Service.InitializeApplication(applicationDto));
        }

        [Fact]
        public async Task GetOrCreateCompany_ShouldCreateNewCompany_WhenItDoesNotExist()
        {
            // Arrange
            var companyDto = new CompanyDTO { Email = "test@company.com" };

            _fixture.MockUnitOfWork.Setup(u => u.Companies.FindByEmailAsync(companyDto.Email)).ReturnsAsync(value: null);

            // Act
            var company = await _fixture.Service.GetOrCreateCompany(companyDto);

            // Assert
            company.Should().NotBeNull();
            company.Email.Should().Be(companyDto.Email);
            _fixture.MockUnitOfWork.Verify(u => u.Companies.Add(It.IsAny<Company>()), Times.Once);
        }

        [Fact]
        public async Task UpdateApplicationParticipants_ShouldAddNewParticipants()
        {
            // Arrange
            var application = new Application { ApplicationParticipants = new List<ApplicationParticipant>() };
            var applicationDto = new ApplicationDTO
            {
                ParticipantDTOs = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Email = "test@test.com" }
                }
            };

            _fixture.MockUnitOfWork.Setup(u => u.Participants.FindByEmailAsync("test@test.com")).ReturnsAsync(value: null);

            // Act
            await _fixture.Service.UpdateApplicationParticipants(application, applicationDto);

            // Assert
            application.ApplicationParticipants.Should().HaveCount(1);
            _fixture.MockUnitOfWork.Verify(u => u.Participants.Add(It.IsAny<Participant>()), Times.Once);
        }

        [Fact]
        public async Task UpdateApplicationParticipants_ShouldNotAddParticipantAlreadyAssociated()
        {
            // Arrange
            var application = new Application
            {
                ApplicationParticipants = new List<ApplicationParticipant>()
                {
                    new ApplicationParticipant { Participant = new Participant { Email = "test@test.com" } }
                }
            };
            var applicationDto = new ApplicationDTO
            {
                ParticipantDTOs = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Email = "test@test.com" }
                }
            };

            _fixture.MockUnitOfWork.Setup(u => u.Participants.FindByEmailAsync("existing@example.com")).ReturnsAsync(new Participant { Email = "existing@example.com" });

            // Act
            await _fixture.Service.UpdateApplicationParticipants(application, applicationDto);

            // Assert
            application.ApplicationParticipants.Should().HaveCount(1); // No new participants added
            _fixture.MockUnitOfWork.Verify(u => u.Participants.Add(It.IsAny<Participant>()), Times.Never); // Verify no new participant was added
        }

        [Fact]
        public async Task UpdateApplicationParticipants_ShouldRemoveUnlistedParticipants()
        {
            // Arrange
            var participantToRemove = new Participant { Email = "remove@example.com", ParticipantId = 2 };
            var mockApplicationParticipantRepo = new Mock<IApplicationParticipantRepository>();

            _fixture.MockUnitOfWork.Setup(u => u.ApplicationParticipants).Returns(mockApplicationParticipantRepo.Object);
            var application = new Application
            {
                ApplicationParticipants = new List<ApplicationParticipant>
                {
                    new ApplicationParticipant { Participant = participantToRemove, ApplicationId = 1, ParticipantId = 2 }
                }
            };
            var applicationDto = new ApplicationDTO
            {
                ParticipantDTOs = new List<ParticipantDTO>() // Empty list implies removing existing participants
            };

            // Act
            await _fixture.Service.UpdateApplicationParticipants(application, applicationDto);

            // Assert
            application.ApplicationParticipants.Should().BeEmpty(); // Participant should be removed
            _fixture.MockUnitOfWork.Verify(u => u.ApplicationParticipants.Remove(It.IsAny<ApplicationParticipant>()), Times.Once); // Verify removal
        }

        [Fact]
        public async Task UpdateApplicationParticipants_ShouldCorrectlyUpdateParticipants()
        {
            // Arrange
            var application = new Application
            {
                ApplicationId = 1,
                ApplicationParticipants = new List<ApplicationParticipant>
                {
                    new ApplicationParticipant { Participant = new Participant { Email = "keep@participant.com", ParticipantId = 1 } }
                }
            };
            var applicationDto = new ApplicationDTO
            {
                ParticipantDTOs = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Email = "keep@participant.com" },
                    new ParticipantDTO { Email = "addnew@participant.com" }
                }
            };

            _fixture.MockUnitOfWork.Setup(u => u.Participants.FindByEmailAsync("keep@participant.com")).ReturnsAsync(new Participant { Email = "keep@participant.com", ParticipantId = 1 });
            _fixture.MockUnitOfWork.Setup(u => u.Participants.FindByEmailAsync("addnew@participant.com")).ReturnsAsync(value: null);

            // Act
            await _fixture.Service.UpdateApplicationParticipants(application, applicationDto);

            // Assert
            application.ApplicationParticipants.Should().ContainSingle(ap => ap.Participant.Email == "keep@participant.com");
            application.ApplicationParticipants.Should().ContainSingle(ap => ap.Participant.Email == "addnew@participant.com");
            _fixture.MockUnitOfWork.Verify(u => u.Participants.Add(It.Is<Participant>(p => p.Email == "addnew@participant.com")), Times.Once);
        }

        [Fact]
        public async Task UpdateApplicationParticipants_ShouldNotModifyParticipants()
        {
            //Arrange
            var existingParticipants = new List<Participant>
            {
                new Participant { Email = "participant@test.com", ParticipantId = 1 }
            };
            _fixture.MockUnitOfWork.Setup(u => u.Participants.FindByEmailAsync("participant@test.com")).ReturnsAsync(existingParticipants.First());

            var application = new Application
            {
                ApplicationParticipants = existingParticipants.Select(p => new ApplicationParticipant { Participant = p }).ToList()
            };

            var applicationDto = new ApplicationDTO
            {
                ParticipantDTOs = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Email = "participant@test.com", Name = "Name", PhoneNumber = "123123123" }
                }
            };

            // Act
            await _fixture.Service.UpdateApplicationParticipants(application, applicationDto);

            //Assert
            _fixture.MockUnitOfWork.Verify(u => u.Participants.Add(It.IsAny<Participant>()), Times.Never);
            _fixture.MockUnitOfWork.Verify(u => u.ApplicationParticipants.Remove(It.IsAny<ApplicationParticipant>()), Times.Never);
        }
    }
}