// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskProcessorTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TaskProcessorTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System.Collections.Generic;

    using Core;
    using Domain;

    using Moq;

    using NUnit.Framework;

    using System;

    /// <summary>
    /// The task processor tests.
    /// </summary>
    [TestFixture]
    public class TaskProcessorTests
    {
        IList<HumanTask> tasks = new List<HumanTask>
                {
                    new HumanTask{ Id = 1, Name = "First Task"},
                    new HumanTask{Id = 2, Name = "Second Task"},
                    new HumanTask{Id = 3, Name = "Third Task"}
                };

        private Mock<IHumanTaskRepository> mockHumanTaskRepository;
        private Mock<IReminderRepository> mockReminderRepository;
        private TaskProcessor processorUnderTest;
        private Mock<IUserRepository> mockUserRepository;

        [SetUp]
        public void TaskProcessorTestsSetup()
        {
            mockHumanTaskRepository = new Mock<IHumanTaskRepository>();

            //mockHumanTaskRepository.Setup(mr => mr.GetAll()).Returns(tasks);

            //mockHumanTaskRepository.Setup(mr => mr.GetAllTasksForEmployee(It.IsAny<int>())).Returns((int id) =>
            //    tasks.Where(x => x.AssigneeId == id));

            //mockHumanTaskRepository.Setup(mr => mr.GetAllTasksForEmployee(It.IsAny<int>())).Returns((int id) =>
            //    tasks.Where(x => x.CreatorId == id));

            //mockHumanTaskRepository.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int id) =>
            //    tasks.Where(x => x.Id == id).Single());

            //mockHumanTaskRepository.Setup(mr => mr.Add(It.IsAny<HumanTask>())).Returns((HumanTask target) =>
            //{
            //    target.Id = tasks.Count() + 1;
            //    tasks.Add(target);
            //    return target;
            //});

            //mockHumanTaskRepository.Setup(it => it.Delete(It.IsAny<int>())).Callback<int>(id =>
            //{
            //    var ttr = tasks.FirstOrDefault(it => it.Id == id);
            //    if (ttr != null)
            //    {
            //        tasks.Remove(ttr);
            //    }
            //});

            this.mockReminderRepository = new Mock<IReminderRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();
            this.processorUnderTest = new TaskProcessor(this.mockHumanTaskRepository.Object, this.mockReminderRepository.Object, mockUserRepository.Object);
        }

        [Test]
        public void Should_AddTask()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };

            this.processorUnderTest.CreateTask(testTask);
            this.mockHumanTaskRepository.Verify(it => it.Add(testTask), Times.Once());
        }

        [Test]
        public void Should_AddTaskWithReminder()
        {
            // arrange 

            const int ExpectedTaskIdAfterSave = 777;

            this.mockHumanTaskRepository.Setup(it => it.Add(It.IsAny<HumanTask>())).Callback<HumanTask>((task) =>
            {
                task.Id = ExpectedTaskIdAfterSave;
            });

            // act
            this.processorUnderTest.CreateTask(
                new HumanTask { Description = "bla bla" },
                new Reminder
                    {
                        ReminderDate = new DateTime(1234, 1, 1)
                    });

            // assert
            this.mockHumanTaskRepository.Verify(it => it.Add(
                It.Is<HumanTask>(x => x.Description == "bla bla")));

            this.mockReminderRepository.Verify(it => it.Add(
                It.Is<Reminder>(x => x.TaskId == ExpectedTaskIdAfterSave)));

        }

        /*[Test]
        public void Should_AssignTask_WhenSuchEmployeeExists()
        {
            //arrange
            mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            mockEmployeeRepository.Setup(it => it.GetById(3)).Returns(new Employee { Id = 3 });

            //act
            processorUnderTest.AssignTask(1, 3);

            //assert
            mockHumanTaskRepository.Verify(it => it.Update(
                It.Is<HumanTask>(x => x.AssigneeId == 3)), Times.Once());
        }*/

        /*[Test]
        public void ShouldNot_AssignTask_WhenSuchEmployeeDoesNotExist()
        {
            //arrange
            mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            mockEmployeeRepository.Setup(it => it.GetById(4)).Throws<InvalidOperationException>();

            //act
            processorUnderTest.AssignTask(1, 4);

            //assert
            mockHumanTaskRepository.Verify(it => it.Update(
                It.Is<HumanTask>(x => x.AssigneeId == 4)), Times.Never());

        }*/
        [Test]
        public void Should_AssignTask_WhenSuchUserExists()
        {
            //arrange
            this.mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            this.mockUserRepository.Setup(it => it.GetById(3)).Returns(new User { Id = 3 });

            //act
            this.processorUnderTest.MoveTask(1, 3);

            //assert
            this.mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.AssigneeId == 3)), Times.Once());
        }

        [Test]
        public void ShouldNot_AssignTask_WhenSuchUserDoesNotExist()
        {
            //arrange
            this.mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            this.mockUserRepository.Setup(it => it.GetById(5)).Throws<InvalidOperationException>();

            //act
            this.processorUnderTest.MoveTask(1, 5);

            //assert
            this.mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.AssigneeId == 5)), Times.Never());
        }

        [Test]
        public void Should_UpdateTask_WhenTaskIsTheOnlyArgumentOfUpdate()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };

            this.processorUnderTest.UpdateTask(testTask);

            this.mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());

        }

        [Test]
        public void Should_UpdateTaskAndReminder_WhenUpdateArgumentsAreTaskAndReminder()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };
            var testReminder = new Reminder { Id = 2, TaskId = 4 };

            this.processorUnderTest.UpdateTask(testTask, testReminder);

            this.mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());
            this.mockReminderRepository.Verify(it => it.Update(testReminder), Times.Once());

        }

        [Test]
        public void Should_DeleteTask()
        {
            this.processorUnderTest.DeleteTask(1);

            this.mockHumanTaskRepository.Verify(it => it.Delete(1), Times.Once());
        }

        [Test]
        public void Should_DeleteAllReminders_WhenTheyAreRelatedToDeletingTask()
        {
            //arrange
            const int DeletingTask = 4;

            this.mockReminderRepository.Setup(it => it.GetAll()).Returns(
                new List<Reminder>
                    {
                        new Reminder() { TaskId = 3 },
                        new Reminder() { TaskId = DeletingTask },
                        new Reminder() { TaskId = 2 }
                    });

            // act
            this.processorUnderTest.DeleteTask(DeletingTask);

            //assert
            this.mockReminderRepository.Verify(it => it.Delete(It.IsAny<Reminder>()), Times.AtLeastOnce());
        }

        [Test]
        public void ShouldNot_DeleteAnyReminders_WhenTheyAreNotRelatedToDeletingTask()
        {
            //arrange
            const int DeletingTask = 4;

            this.mockReminderRepository.Setup(it => it.GetAll()).Returns(
                new List<Reminder>
                    {
                        new Reminder { TaskId = 3 }, 
                        new Reminder { TaskId = 5 }, 
                        new Reminder { TaskId = 2 }, 
                    });

            // act
            this.processorUnderTest.DeleteTask(DeletingTask);

            // assert
            this.mockReminderRepository.Verify(it => it.Delete(It.IsAny<Reminder>()), Times.Never());
        }

        [Test]
        public void Should_ReturnListOfTasksOfEmployeeByHisId()
        {
            this.processorUnderTest.GetTaskById(1);

            this.mockHumanTaskRepository.Verify(it => it.GetById(1), Times.Once());
        }

        [Test]
        public void Should_ReturnListOfAllTasks_WhenGetTasksListIsCalledWithNoArgumentsIsCalled()
        {
            this.processorUnderTest.GetTasksList();

            this.mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }

        [Test]
        public void Should_UpdateTaskClosedFieldWithCurrentDate_WhenCloseIsCalled()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });

            // act
            this.processorUnderTest.CloseTask(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.Closed != null)), Times.Once());
        }

        [Test]
        public void Should_UpdateTaskAndDeleteRelatedReminders_WhenMoveTaskIsCalled()
        {
            // arrange
            const int TaskBeingMoved = 1;
            this.mockReminderRepository.Setup(it => it.GetAll()).Returns(
                new List<Reminder>
                    {
                        new Reminder() { TaskId = 3 },
                        new Reminder() { TaskId = TaskBeingMoved },
                        new Reminder() { TaskId = 2 },
                    });
            this.mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });

            // act
            this.processorUnderTest.MoveTask(TaskBeingMoved, 4);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.AssigneeId == 4)), Times.Once());
            this.mockReminderRepository.Verify(
                it => it.Delete(It.Is<Reminder>(x => x.TaskId == TaskBeingMoved)), Times.AtLeastOnce());
        }
    }
}
