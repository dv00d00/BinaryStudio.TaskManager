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

            mockReminderRepository = new Mock<IReminderRepository>();
            this.mockUserRepository = this.mockUserRepository;
            processorUnderTest = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object, mockUserRepository.Object);
        }

        [Test]
        public void Should_AddTask()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };

            processorUnderTest.CreateTask(testTask);
            mockHumanTaskRepository.Verify(it => it.Add(testTask), Times.Once());
        }

        [Test]
        public void Should_AddTaskWithReminder()
        {
            // arrange 

            const int expectedTaskIdAfterSave = 777;

            mockHumanTaskRepository.Setup(it => it.Add(It.IsAny<HumanTask>())).Callback<HumanTask>((task) =>
            {
                task.Id = expectedTaskIdAfterSave;
            });

            // act
            processorUnderTest.CreateTask(new HumanTask() { Description = "bla bla" }, 
                new Reminder() { ReminderDate = new DateTime(1234, 1, 1) });

            // assert

            mockHumanTaskRepository.Verify(it => it.Add(
                It.Is<HumanTask>(x => x.Description == "bla bla")));

            mockReminderRepository.Verify(it => it.Add(
                It.Is<Reminder>(x => x.TaskId == expectedTaskIdAfterSave)));

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
            mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            mockUserRepository.Setup(it => it.GetById(3)).Returns(new User{ Id = 3 });

            //act
            processorUnderTest.MoveTask(1, 3);

            //assert
            mockHumanTaskRepository.Verify(it => it.Update(
                It.Is<HumanTask>(x => x.AssigneeId == 3)), Times.Once());
        }

        [Test]
        public void ShouldNot_AssignTask_WhenSuchUserDoesNotExist()
        {
            //arrange
            mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            mockUserRepository.Setup(it => it.GetById(5)).Throws<InvalidOperationException>();

            //act
            processorUnderTest.MoveTask(1, 5);

            //assert
            mockHumanTaskRepository.Verify(it => it.Update(
                It.Is<HumanTask>(x => x.AssigneeId == 5)), Times.Never());

        }

        [Test]
        public void Should_UpdateTask_WhenTaskIsTheOnlyArgumentOfUpdate()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };

            processorUnderTest.UpdateTask(testTask);

            mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());

        }

        [Test]
        public void Should_UpdateTaskAndReminder_WhenUpdateArgumentsAreTaskAndReminder()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };
            var testReminder = new Reminder { Id = 2, TaskId = 4 };

            processorUnderTest.UpdateTask(testTask,testReminder);

            mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());
            mockReminderRepository.Verify(it => it.Update(testReminder), Times.Once());

        }

        [Test]
        public void Should_DeleteTask()
        {
            processorUnderTest.DeleteTask(1);

            mockHumanTaskRepository.Verify(it => it.Delete(1), Times.Once());
        }

        [Test]
        public void Should_DeleteAllReminders_WhenTheyAreRelatedToDeletingTask()
        {
            //arrange
            const int deletingTask = 4;

            mockReminderRepository.Setup(it => it.GetAll()).Returns(new List<Reminder>{
                new Reminder(){TaskId = 3}, 
                new Reminder(){TaskId = deletingTask},
                new Reminder(){TaskId = 2}
            });

            //act
            processorUnderTest.DeleteTask(deletingTask);

            //assert
            mockReminderRepository.Verify(it => it.Delete(It.IsAny<Reminder>()), Times.AtLeastOnce());
        }

        [Test]
        public void ShouldNot_DeleteAnyReminders_WhenTheyAreNotRelatedToDeletingTask()
        {
            //arrange
            const int deletingTask = 4;

            mockReminderRepository.Setup(it => it.GetAll()).Returns(new List<Reminder>{
                new Reminder(){TaskId = 3}, 
                new Reminder(){TaskId = 5},
                new Reminder(){TaskId = 2},
            });

            //act
            processorUnderTest.DeleteTask(deletingTask);

            //assert
            mockReminderRepository.Verify(it => it.Delete(It.IsAny<Reminder>()), Times.Never());
        }

        [Test]
        public void Should_ReturnListOfTasksOfEmployeeByHisId()
        {
            processorUnderTest.GetTaskById(1);

            mockHumanTaskRepository.Verify(it => it.GetById(1), Times.Once());
        }

        [Test]
        public void Should_ReturnListOfAllTasks_WhenGetTasksListIsCalledWithNoArgumentsIsCalled()
        {
            processorUnderTest.GetTasksList();

            mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }

        [Test]
        public void Should_UpdateTaskClosedFieldWithCurrentDate_WhenCloseIsCalled()
        {
            //arrange
            mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
          
            //act
            processorUnderTest.CloseTask(1);

            //assert
            mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.Closed!=null)), Times.Once());
        }

        [Test]
        public void Should_UpdateTaskAndDeleteRelatedReminders_WhenMoveTaskIsCalled()
        {
            //arrange
            var taskBeingMoved = 1;
            mockReminderRepository.Setup(it => it.GetAll()).Returns(new List<Reminder>{
                new Reminder(){TaskId = 3}, 
                new Reminder(){TaskId = taskBeingMoved},
                new Reminder(){TaskId = 2},
            });
            mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });

            //act
            processorUnderTest.MoveTask(taskBeingMoved,4);

            //assert
            mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.AssigneeId == 4)),Times.Once());
            mockReminderRepository.Verify(it => it.Delete(It.Is<Reminder>(x => x.TaskId == taskBeingMoved)), Times.AtLeastOnce());
        }


    }
}
