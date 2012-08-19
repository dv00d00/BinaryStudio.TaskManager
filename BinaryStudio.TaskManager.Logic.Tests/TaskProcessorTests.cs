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
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// The task processor tests.
    /// </summary>
    [TestFixture]
    public class TaskProcessorTests
    {
        /// <summary>
        /// The tasks.
        /// </summary>
        public IList<HumanTask> tasks = new List<HumanTask>
            {
                new HumanTask { Id = 1, Name = "First Task" },
                new HumanTask { Id = 2, Name = "Second Task" },
                new HumanTask{ Id = 3, Name = "Third Task" }
                };

        /// <summary>
        /// The mock human task repository.
        /// </summary>
        private Mock<IHumanTaskRepository> mockHumanTaskRepository;

        /// <summary>
        /// The mock reminder repository.
        /// </summary>
        private Mock<IReminderProcessor> mockReminderProcessor;

        /// <summary>
        /// The processor under test.
        /// </summary>
        private TaskProcessor processorUnderTest;

        /// <summary>
        /// The mock user repository.
        /// </summary>
        private Mock<IUserRepository> mockUserRepository;

        /// <summary>
        /// The task processor tests setup.
        /// </summary>
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

            this.mockReminderProcessor = new Mock<IReminderProcessor>();
            this.mockUserRepository = new Mock<IUserRepository>();
            this.processorUnderTest = new TaskProcessor(this.mockHumanTaskRepository.Object, this.mockReminderProcessor.Object, mockUserRepository.Object);
        }

        /// <summary>
        /// The should_ add task.
        /// </summary>
        [Test]
        public void Should_AddTask()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };

            this.processorUnderTest.CreateTask(testTask);
            this.mockHumanTaskRepository.Verify(it => it.Add(testTask), Times.Once());
        }

        /// <summary>
        /// The should_ add task with reminder.
        /// </summary>
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

            this.mockReminderProcessor.Verify(it => it.AddReminder(
                It.Is<Reminder>(x => x.TaskId == ExpectedTaskIdAfterSave)));
        }

        /// <summary>
        /// The should_ assign task_ when such user exists.
        /// </summary>
        [Test]
        public void Should_AssignTask_WhenSuchUserExists()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            this.mockUserRepository.Setup(it => it.GetById(3)).Returns(new User { Id = 3 });

            // act
            this.processorUnderTest.MoveTask(1, 3);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.AssigneeId == 3)), Times.Once());
        }

        /// <summary>
        /// The should not_ assign task_ when such user does not exist.
        /// </summary>
        [Test]
        public void ShouldNot_AssignTask_WhenSuchUserDoesNotExist()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(it => it.GetById(1)).Returns(new HumanTask { Id = 1 });
            this.mockUserRepository.Setup(it => it.GetById(5)).Throws<InvalidOperationException>();

            // act
            this.processorUnderTest.MoveTask(1, 5);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.Update(It.Is<HumanTask>(x => x.AssigneeId == 5)), Times.Never());
        }

        /// <summary>
        /// The should_ update task_ when task is the only argument of update.
        /// </summary>
        [Test]
        public void Should_UpdateTask_WhenTaskIsTheOnlyArgumentOfUpdate()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };

            this.processorUnderTest.UpdateTask(testTask);

            this.mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());
        }

        /// <summary>
        /// The should_ update task and reminder_ when update arguments are task and reminder.
        /// </summary>
        [Test]
        public void Should_UpdateTaskAndReminder_WhenUpdateArgumentsAreTaskAndReminder()
        {
            var testTask = new HumanTask { Id = 4, Name = "Fourth Task" };
            var testReminder = new Reminder { Id = 2, TaskId = 4 };

            this.processorUnderTest.UpdateTask(testTask, testReminder);

            this.mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());
            this.mockReminderProcessor.Verify(it => it.UpdateReminder(testReminder), Times.Once());
        }

        /// <summary>
        /// The should_ delete task.
        /// </summary>
        [Test]
        public void Should_DeleteTask()
        {
            this.processorUnderTest.DeleteTask(1);

            this.mockHumanTaskRepository.Verify(it => it.Delete(1), Times.Once());
        }

        /// <summary>
        /// The should_ delete all reminders_ when they are related to deleting task.
        /// </summary>
        [Test]
        public void Should_DeleteAllReminders_WhenTheyAreRelatedToDeletingTask()
        {
            // arrange
            const int DeletingTask = 4;

            this.mockReminderProcessor.Setup(it => it.GetAll()).Returns(
                new List<Reminder>
                    {
                        new Reminder() { TaskId = 3 },
                        new Reminder() { TaskId = DeletingTask },
                        new Reminder() { TaskId = 2 }
                    });

            // act
            this.processorUnderTest.DeleteTask(DeletingTask);

            // assert
            this.mockReminderProcessor.Verify(it => it.DeleteRemindersForTask(DeletingTask), Times.AtLeastOnce());
        }

        /// <summary>
        /// The should not_ delete any reminders_ when they are not related to deleting task.
        /// </summary>
        [Test]
        public void ShouldNot_DeleteAnyReminders_WhenTheyAreNotRelatedToDeletingTask()
        {
            // arrange
            const int DeletingTask = 4;

            this.mockReminderProcessor.Setup(it => it.GetAll()).Returns(
                new List<Reminder>
                    {
                        new Reminder { TaskId = 3 }, 
                        new Reminder { TaskId = 5 }, 
                        new Reminder { TaskId = 2 }, 
                    });

            // act
            this.processorUnderTest.DeleteTask(DeletingTask);

            // assert
            this.mockReminderProcessor.Verify(it => it.DeleteRemindersForTask(DeletingTask), Times.Never());
        }

        /// <summary>
        /// The should_ return list of tasks of employee by his id.
        /// </summary>
        [Test]
        public void Should_ReturnListOfTasksOfEmployeeByHisId()
        {
            this.processorUnderTest.GetTaskById(1);

            this.mockHumanTaskRepository.Verify(it => it.GetById(1), Times.Once());
        }

        /// <summary>
        /// The should_ return list of tasks for project by its id.
        /// </summary>
        [Test]
        public void Should_ReturnListOfTasksForProjectByItsId()
        {
            // act
            this.processorUnderTest.GetAllTasksInProject(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllTasksInProject(1), Times.Once());
        }

        /// <summary>
        /// The should_ return list of all tasks_ when get tasks list is called with no arguments is called.
        /// </summary>
        [Test]
        public void Should_ReturnListOfAllTasks_WhenGetTasksListIsCalledWithNoArgumentsIsCalled()
        {
            this.processorUnderTest.GetTasksList();

            this.mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }

        /// <summary>
        /// The should_ update task closed field with current date_ when close is called.
        /// </summary>
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

        /// <summary>
        /// The should_ update task and delete related reminders_ when move task is called.
        /// </summary>
        [Test]
        public void Should_UpdateTask_WhenMoveTaskIsCalled()
        {
            // arrange
            const int TaskBeingMoved = 1;
            this.mockReminderProcessor.Setup(it => it.GetAll()).Returns(
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
            //this.mockReminderProcessor.Verify(
              //  it => it.DeleteReminder(It.Is<Reminder>(x => x.TaskId == TaskBeingMoved).Id), Times.AtLeastOnce());
        }

        /// <summary>
        /// The should_ return list of unassigned tasks in project with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnListOfUnassignedTasksInProjectWithId1()
        {
            // act
            this.processorUnderTest.GetUnAssignedTasksForProject(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetUnassingnedTasks(1), Times.Once());
        }

        /// <summary>
        /// The should_ return history for task with id 1 in project.
        /// </summary>
        [Test]
        public void Should_ReturnHistoryForTaskWithId1InProject()
        {
            // act
            this.processorUnderTest.GetAllHistoryForTask(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllHistoryForTask(1), Times.Once());
        }

        /// <summary>
        /// The should_ return list of tasks for user with id 2 project with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnListOfTasksForUserWithId2ProjectWithId1()
        {
            // act
            this.processorUnderTest.GetAllTasksForUserInProject(1, 2);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllTasksForUserInProject(1, 2), Times.Once());
        }

        /// <summary>
        /// The should_ return task history for user with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnTaskHistoryForUserWithId1()
        {
            // act
            this.processorUnderTest.GetAllHistoryForUser(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllHistoryForUser(1), Times.Once());
        }

        /// <summary>
        /// The should_ return all closed tasks in project with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnAllClosedTasksInProjectWithId1()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(x => x.GetAllTasksInProject(1)).Returns(new List<HumanTask>());

            // act
            this.processorUnderTest.GetAllClosedTasksForProject(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllTasksInProject(1), Times.Once());
        }

        /// <summary>
        /// The should_ return all closed tasks for user with id 2 in project with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnAllClosedTasksForUserWithId2InProjectWithId1()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(x => x.GetAllTasksForUserInProject(1, 2)).Returns(new List<HumanTask>());

            // act
            this.processorUnderTest.GetAllClosedTasksForUserInProject(1, 2);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllTasksForUserInProject(1, 2), Times.Once());
        }

        /// <summary>
        /// The should_ return all opened tasks in project with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnAllOpenedTasksInProjectWithId1()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(x => x.GetAllTasksInProject(1)).Returns(new List<HumanTask>());

            // act
            this.processorUnderTest.GetAllOpenTasksForProject(1);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllTasksInProject(1), Times.Once());
        }

        /// <summary>
        /// The should_ return all opened tasks for user with id 2 in project with id 1.
        /// </summary>
        [Test]
        public void Should_ReturnAllOpenedTasksForUserWithId2InProjectWithId1()
        {
            // arrange
            this.mockHumanTaskRepository.Setup(x => x.GetAllTasksForUserInProject(1, 2)).Returns(new List<HumanTask>());

            // act
            this.processorUnderTest.GetAllOpenTasksForUserInProject(1, 2);

            // assert
            this.mockHumanTaskRepository.Verify(it => it.GetAllTasksForUserInProject(1, 2), Times.Once());
        }
    }
}
