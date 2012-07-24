﻿namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class HumanTaskControllerTests
    {
        private Mock<ITaskProcessor> taskProcessorMock;
        private HumanTasksController controller;
        private Mock<IUserProcessor> userProcessorMock;

        [SetUp]
        public void Init()
        {
            this.taskProcessorMock = new Mock<ITaskProcessor>();
            this.userProcessorMock = new Mock<IUserProcessor>();
            this.userRepositoryMock = new Mock<IUserRepository>();

            this.controller = new HumanTasksController(
                this.taskProcessorMock.Object,
                this.userProcessorMock.Object,
                this.userRepositoryMock.Object);
        }

        private Mock<IUserRepository> userRepositoryMock;

        [Test]
        public void Should_CreateAssignedTask_WhenMethodCreateCalledWithAssignedId()
        {
            //act
            this.controller.Create(new HumanTask() { AssigneeId = 5 });

            //assert            
            this.taskProcessorMock.Verify(x => x.CreateTask(It.Is<HumanTask>(it => it.AssigneeId == 5)), Times.Once());
        }

        [Test]
        public void Should_GetAllTasksFromTaskProcessor_WhenControllerPrepareMainPage()
        {
            //act
            controller.AllTasks();

            //assert
            this.taskProcessorMock.Verify(x => x.GetAllTasks(), Times.Once());
        }

        [Test]
        public void Should_GetTaskFromTaskProcessor_WhenMustBeShownTaskDetails()
        {
            //arrange
            this.taskProcessorMock.Setup(x => x.GetTaskById(3)).Returns(new HumanTask { Id = 3 });

            //act
            controller.Details(3);

            //assert
            this.taskProcessorMock.Verify(x => x.GetTaskById(3), Times.Once());
        }

         [Test]
        public void Should_GetTaskWithId5FromTaskProcessor_WhenTaskWasEdited()
        {
            //act
            controller.Edit(5);

            //assert
            this.taskProcessorMock.Verify(x=>x.GetTaskById(5), Times.Once());
        }

        [Test]
        public void Should_UpdateTaskFromTaskProcessor_WhenTaskWithId5WasEdited()
        {
            //act
            this.controller.Edit(new HumanTask(){Id = 5});
            
            //assert
            this.taskProcessorMock.Verify(x=>x.UpdateTask(It.Is<HumanTask>(it=>it.Id==5)),Times.Once());
        }

        [Test]
        public void Should_GetTaskWithId5FromTaskProcessor_WhenTaskWasDeleted()
        {
            //arrange
            this.taskProcessorMock.Setup(x => x.GetTaskById(5)).Returns(new HumanTask {Id = 5});
            
            //act
            controller.Delete(5);

            //assert
            this.taskProcessorMock.Verify(x => x.GetTaskById(5), Times.Once());
        }

        [Test]
        public void Should_DeleteTaskWithId5FromTaskProcessor()
        {
            //act
            this.controller.DeleteConfirmed(5);

            //assert
            this.taskProcessorMock.Verify(x => x.DeleteTask(5), Times.Once());
        }

        [Test]
        public void Should_MoveTaskWithId5ToAssignedEmployeeWithId3TasksList()
        {
            //arrange
            const int taskId=5, senderId=1, receiverId=3;

            //act
            this.controller.MoveTask(taskId, senderId, receiverId);

            //assert
            this.taskProcessorMock.Verify(x => x.MoveTask(taskId, receiverId), Times.AtLeastOnce());
        }

        [Test]
        public void Should_MoveTaskWithId5ToUnassignedEmployees()
        {
            //arrange
            const int taskId = 5, senderId = 1, receiverId = -1;

            //act
            this.controller.MoveTask(taskId, senderId, receiverId);

            //assert
            this.taskProcessorMock.Verify(x => x.MoveTaskToUnassigned(taskId), Times.AtLeastOnce());
        }
    }
}