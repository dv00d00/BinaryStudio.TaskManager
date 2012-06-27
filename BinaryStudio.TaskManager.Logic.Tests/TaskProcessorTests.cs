using System;

namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Core;
    using Domain;

    using Moq;

    using NUnit.Framework;

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
        private Mock<IEmployeeRepository> employeeRepository;

        [SetUp]
        public void TaskProcessorTestsSetup()
        {
            mockHumanTaskRepository = new Mock<IHumanTaskRepository>();

            //mockHumanTaskRepository.Setup(mr => mr.GetAll()).Returns(tasks);

            //mockHumanTaskRepository.Setup(mr => mr.GetAllForEmployee(It.IsAny<int>())).Returns((int id) =>
            //    tasks.Where(x => x.AssigneeId == id));

            //mockHumanTaskRepository.Setup(mr => mr.GetAllForEmployee(It.IsAny<int>())).Returns((int id) =>
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

            employeeRepository = new Mock<IEmployeeRepository>();

            processorUnderTest = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object);
        }

        [Test]
        public void Should_AddTask()
        {
            var testTask = new HumanTask{Id = 4, Name = "Fourth Task"};

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
            processorUnderTest.CreateTask(new HumanTask(){Description = "bla bla"}, new Reminder(){ReminderDate = new DateTime(1234,1,1)});

            // assert

            mockHumanTaskRepository.Verify(it => it.Add(It.Is<HumanTask>(x => x.Description == "bla bla")));

            mockReminderRepository.Verify(it => it.Add(It.Is<Reminder>(x => x.TaskId == expectedTaskIdAfterSave)));

        }

        [Test]
        [ExpectedException]
        public void Should_AssignTask_WhenSuchEmployeExists()
        {
     /*       TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);
            
            processor.AssignTask(1,5);
            mockHumanTaskRepository.Verify(it => it.(), Times.Once());*/
        }

        [Test]
        public void ShouldNot_AssignTask_WhenSuchEmployeeDoesNotExist()
        {

        }

        [Test]
        public void Should_UpdateTask_WhenManagerIsTrying()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object);
            
            var testTask = new HumanTask{Id = 4, Name = "Fourth Task"};

            processor.UpdateTask(testTask);
            mockHumanTaskRepository.Verify(it => it.Update(testTask), Times.Once());

        }

        [Test]
        public void ShouldNot_UpdateTask_WhenItIsAlreadyDone()
        {

        }

        [Test]
        public void Should_DeleteTask()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object);
            
            processor.DeleteTask(1);
            mockHumanTaskRepository.Verify(it => it.Delete(1), Times.Once());
        }

        [Test]
        public void Should_ReturnListOfTasksOfEmployeeByHisId()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object);

            processor.GetTaskById(1);
            mockHumanTaskRepository.Verify(it => it.GetById(1), Times.Once());
        }

        public void Should_ReturnListOfTasksOfAllTasks()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object);

            processor.GetTaskById(1);
            mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }

        [Test]
        public void Should_ReturnListOfNotassignedTasks()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object, mockReminderRepository.Object);

            processor.GetTasksList();
            mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }
    }
}
