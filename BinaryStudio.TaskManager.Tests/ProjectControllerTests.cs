using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using BinaryStudio.TaskManager.Web.Controllers;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Web.Tests
{
    [TestFixture]
    public class ProjectControllerTests
    {
        private Mock<ITaskProcessor> taskProcessorMock;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IUserProcessor> userProcessorMock;
        private Mock<IProjectRepository> projectRepositoryMock;
        private ProjectController controller;
        private Mock<IProjectProcessor> projectProcessorMock;

        [SetUp]
        public void Init()
        {
            this.taskProcessorMock = new Mock<ITaskProcessor>();
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userProcessorMock = new Mock<IUserProcessor>();
            this.projectRepositoryMock = new Mock<IProjectRepository>();
            this.projectProcessorMock = new Mock<IProjectProcessor>();
            this.controller = new ProjectController(
                this.taskProcessorMock.Object,
                this.userProcessorMock.Object,
                this.userRepositoryMock.Object,
                this.projectRepositoryMock.Object,
                this.projectProcessorMock.Object);
        }

        [Test]
        public void Should_CreateTask_WhenMethodCreateTaskWasCalledWithAssignedId3()
        {
            // act
            this.controller.CreateTask(new HumanTask { AssigneeId = 3 });

            // assert
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 3)), Times.Once());
        }

        [Test]
        public void Should_InviteUserInProject()
        {
            // act
            this.controller.InviteUserInProject(1, 1);

            // assert
            this.projectProcessorMock.Verify(x =>x.InviteUserInProject(1, 1));
        }
    }
}
