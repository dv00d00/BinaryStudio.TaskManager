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
        private ProjectController controller;

        [SetUp]
        public void Init()
        {
            taskProcessorMock = new Mock<ITaskProcessor>();
            userRepositoryMock = new Mock<IUserRepository>();
            userProcessorMock = new Mock<IUserProcessor>();
            controller = new ProjectController(
                this.taskProcessorMock.Object,
                this.userProcessorMock.Object,
                this.userRepositoryMock.Object
                );
        }

        [Test]
        public void Should_CreateTask_WhenMethodCreateTaskWasCalledWithAssignedId3()
        {
            //act
            this.controller.CreateTask(new HumanTask() { AssigneeId = 3 });

            //assert
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 3)), Times.Once());
        }

    }
}
