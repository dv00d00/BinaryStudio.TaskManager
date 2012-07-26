// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectControllerTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProjectControllerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// The project controller tests.
    /// </summary>
    [TestFixture]
    public class ProjectControllerTests
    {
        /// <summary>
        /// The task processor mock.
        /// </summary>
        private Mock<ITaskProcessor> taskProcessorMock;

        /// <summary>
        /// The user repository mock.
        /// </summary>
        private Mock<IUserRepository> userRepositoryMock;

        /// <summary>
        /// The user processor mock.
        /// </summary>
        private Mock<IUserProcessor> userProcessorMock;

        /// <summary>
        /// The project repository mock.
        /// </summary>
        private Mock<IProjectRepository> projectRepositoryMock;

        /// <summary>
        /// The controller.
        /// </summary>
        private ProjectController controller;

        /// <summary>
        /// The init.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.taskProcessorMock = new Mock<ITaskProcessor>();
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userProcessorMock = new Mock<IUserProcessor>();
            this.projectRepositoryMock = new Mock<IProjectRepository>();
            this.controller = new ProjectController(
                this.taskProcessorMock.Object,
                this.userProcessorMock.Object,
                this.userRepositoryMock.Object,
                this.projectRepositoryMock.Object);
        }

        /// <summary>
        /// The should_ create task_ when method create task was called with assigned id 3.
        /// </summary>
        [Test]
        public void Should_CreateTask_WhenMethodCreateTaskWasCalledWithAssignedId3()
        {
            // act
            this.controller.CreateTask(new HumanTask { AssigneeId = 3 });

            // assert
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 3)), Times.Once());
        }
    }
}
