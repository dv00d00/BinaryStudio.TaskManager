using System.Collections;
using System.Collections.Generic;
using System.Linq;

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


        private IList<User> users;
        private User creator;
        private Project project;
        [SetUp]
        public void Setup()
        {
            this.projectRepositoryMock = new Mock<IProjectRepository>();
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.projectProcessor = new ProjectProcessor(this.projectRepositoryMock.Object, this.userRepositoryMock.Object);

            this.users = new List<User>
                                    {
                                        new User
                                            {
                                                Id = 1,
                                                UserName = "User"
                                            }
                                    };

            this.creator = new User
            {
                Id = 1,
                UserName = "Creator"
            };
            this.project = new Project
            {
                Name = "Test project",
                Creator = creator,
                ProjectUsers = users
            };

            this.projectRepositoryMock.Setup(it => it.GetAllUsersInProject(project.Id)).Returns(users);
            this.projectRepositoryMock.Setup(it => it.GetCreatorForProject(project.Id)).Returns(creator);
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
            const int SenderId = 2;
            const int ProjectId = 1;
            const int ReceiverId = 1;

            // act
            this.projectProcessor.InviteUserInProject(SenderId, ProjectId, ReceiverId);

            // assert
            this.projectRepositoryMock.Verify(x => x.AddInvitation(It.Is<Invitation>(it => it.SenderId == SenderId)), Times.Once());
        }

        [Test]
        public void Should_ReturnAllUsers()
        {
            // arrange
            
            
            // act
            var assertValue = this.projectProcessor.GetAllUsersInProject(project.Id).ToList();

            // assert
            Assert.AreEqual(users,assertValue);
        } 


        [Test]
        public void Should_ReturnCreator()
        {
            // arrange
            this.projectRepositoryMock.Setup(it => it.GetById(project.Id)).Returns(project);
            
            // act
            var assertValue = this.projectProcessor.GetCreator(project.Id);

            // assert
            Assert.AreEqual(creator, assertValue);
        }

        [Test]
        public void Should_ReturnProject_WhenTryGetById()
        {

            // arrenge
            this.projectRepositoryMock.Setup(it => it.GetById(project.Id)).Returns(project);
            // act
            var asserProject = this.projectProcessor.GetProjectById(project.Id);

            // assert
            Assert.AreEqual(project,asserProject);
        }
    }
}
