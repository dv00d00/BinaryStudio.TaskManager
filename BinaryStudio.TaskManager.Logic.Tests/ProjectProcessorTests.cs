namespace BinaryStudio.TaskManager.Logic.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectProcessorTests
    {
        [Test]
        public void Should_CreateProject()
        {
            // arrange
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var projectProcessor = new ProjectProcessor(projectRepositoryMock.Object);
            const string Description = "simply description";
            const string ProjectName = "NameProject";
            const int UserId = 1;
            var user = new User { Id = UserId };

            // act
            projectProcessor.CreateProject(user, ProjectName, Description);

            // assert
            projectRepositoryMock.Verify(x => x.Add(It.Is<Project>(it => it.CreatorId == UserId)), Times.Once());
        }
    }
}
