namespace BinaryStudio.TaskManager.Logic.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectProcessorTests
    {
        private Mock<IProjectRepository> projectRepositoryMock;

        private ProjectProcessor projectProcessor;

        private Mock<IUserRepository> userRepositoryMock;


        [SetUp]
        public void Setup()
        {
            this.projectRepositoryMock = new Mock<IProjectRepository>();
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.projectProcessor = new ProjectProcessor(this.projectRepositoryMock.Object, this.userRepositoryMock.Object);
        }

        [Test]
        public void Should_CreateProject()
        {
            // arrange            
            const string Description = "simply description";
            const string ProjectName = "NameProject";
            const int UserId = 1;
            var user = new User { Id = UserId };

            // act
            this.projectProcessor.CreateProject(user, ProjectName, Description);

            // assert
            this.projectRepositoryMock.Verify(x => x.Add(It.Is<Project>(it => it.CreatorId == UserId)), Times.Once());
        }

        [Test]
        public void Should_CreateInvitation()
        {
            const int ProjectId = 1;
            const int UserId = 1;

            // act
            this.projectProcessor.InviteUserInProject(UserId, ProjectId);

            // assert
            this.projectRepositoryMock.Verify(x => x.CreateInvitationUserInProject(It.Is<Invitation>(it => it.UserId == UserId)), Times.Once());
        }                
    }
}
