using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Tests
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ProjectControllerTests
    {
        /// <summary>
        /// The task processor mock.
        /// </summary>
        private Mock<ITaskProcessor> taskProcessorMock;

        /// <summary>
        /// The user processor mock.
        /// </summary>
        private Mock<IUserProcessor> userProcessorMock;

        private ProjectController controller;

        private Mock<IProjectProcessor> projectProcessorMock;

        private Mock<INotifier> notifierMock;

        private Mock<INewsRepository> newsRepositoryMock;

        [SetUp]
        public void Init()
        {
            this.taskProcessorMock = new Mock<ITaskProcessor>();

            this.userProcessorMock = new Mock<IUserProcessor>();
            this.projectProcessorMock = new Mock<IProjectProcessor>();
            this.notifierMock = new Mock<INotifier>();
            this.newsRepositoryMock = new Mock<INewsRepository>();

            this.controller = new ProjectController(
                this.taskProcessorMock.Object,
                this.userProcessorMock.Object,
                this.projectProcessorMock.Object, 
                this.notifierMock.Object,
                this.newsRepositoryMock.Object);

        }

        /// <summary>
        /// The should_ create task_ when method create task was called with assigned id 3.
        /// </summary>
        [Test]
        public void Should_CreateTask_WhenMethodCreateTaskWasCalledWithAssignedId3()
        {
            // act
            this.controller.CreateTask(new CreateTaskViewModel { AssigneeId = 3 });

            // assert
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 3)), Times.Once());
        }

        /// <summary>
        /// The should_ update task from task processor_ when task with id 5 was edited.
        /// </summary>
        [Test]
        public void Should_UpdateTaskFromTaskProcessor_WhenTaskWithId5WasEdited()
        {
            // act
            this.controller.Edit(new HumanTask() { Id = 5 });

            // assert
            this.taskProcessorMock.Verify(x => x.UpdateTask(It.Is<HumanTask>(it => it.Id == 5)), Times.Once());
        }

        /// <summary>
        /// The should_ move task with id 5 to assigned user with id 3 tasks list.
        /// </summary>
        [Test]
        public void Should_MoveTaskWithId5ToAssignedUserWithId3TasksList()
        {
            // arrange
            const int TaskId = 5;
            const int SenderId = 1;
            const int ReceiverId = 3;
            const int projectId = 2;

            // act
            this.controller.MoveTask(TaskId, SenderId, ReceiverId,projectId);

            // assert
            this.taskProcessorMock.Verify(x => x.MoveTask(TaskId, ReceiverId), Times.AtLeastOnce());
        }

        /// <summary>
        /// The should_ move task with id 5 to unassigned employees.
        /// </summary>
        [Test]
        public void Should_MoveTaskWithId5ToUnassignedEmployees()
        {
            // arrange
            const int TaskId = 5;
            const int SenderId = 1;
            const int ReceiverId = -1;
            const int projectId = 2;

            // act
            this.controller.MoveTask(TaskId, SenderId, ReceiverId,projectId);

            // assert
            this.taskProcessorMock.Verify(x => x.MoveTaskToUnassigned(TaskId), Times.AtLeastOnce());
        }

        /// <summary>
        /// The should_ get all tasks from task processor_ when controller get all tasks for current project with id 1 was called.
        /// </summary>
        [Test]
        public void Should_GetAllTasksFromTaskProcessor_WhenControllerGetAllTasksForCurrentProjectWithId1WasCalled()
        {
            // arrange
            const int ProjectId = 1;

            // act
            this.controller.AllTasks(ProjectId);

            // assert
            this.taskProcessorMock.Verify(x => x.GetAllTasksInProject(ProjectId), Times.Once());
        }

        /// <summary>
        /// The should_ get task from task processor_ when must be shown task details.
        /// </summary>
        [Test]
        public void Should_GetTaskFromTaskProcessor_WhenMustBeShownTaskDetails()
        {
            // arrange
            this.taskProcessorMock.Setup(x => x.GetTaskById(3)).Returns(new HumanTask { Id = 3 });
            this.taskProcessorMock.Setup(x => x.GetAllHistoryForTask(3)).Returns(new List<HumanTaskHistory>());

            // act
            this.controller.Details(3);

            // assert
            this.taskProcessorMock.Verify(x => x.GetTaskById(3), Times.Once());
        }

        /// <summary>
        /// The should_ get task with id 5 from task processor_ when task was deleted.
        /// </summary>
        [Test]
        public void Should_GetTaskWithId5FromTaskProcessor_WhenTaskWasDeleted()
        {
            // arrange
            this.taskProcessorMock.Setup(x => x.GetTaskById(5)).Returns(new HumanTask { Id = 5 });
            this.taskProcessorMock.Setup(x => x.GetAllHistoryForTask(5)).Returns(new List<HumanTaskHistory>());

            // act
            this.controller.Delete(5);

            // assert
            this.taskProcessorMock.Verify(x => x.GetTaskById(5), Times.Once());
        }

        /// <summary>
        /// The should_ delete task with id 5 from task processor.
        /// </summary>
        [Test]
        public void Should_DeleteTaskWithId5FromTaskProcessor()
        {
            // act
            this.controller.DeleteConfirmed(5);

            // assert
            this.taskProcessorMock.Verify(x => x.DeleteTask(5), Times.Once());
        }
    }
}
