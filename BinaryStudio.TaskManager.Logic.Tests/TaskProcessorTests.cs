namespace BinaryStudio.TaskManager.Logic.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

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
        Mock<IHumanTaskRepository> mockHumanTaskRepository = new Mock<IHumanTaskRepository>();

        [SetUp]
        public void TaskProcessorTestsSetup()
        {

           /* mockHumanTaskRepository.Setup(mr => mr.GetAll()).Returns(tasks);

            mockHumanTaskRepository.Setup(mr => mr.GetAllForEmployee(It.IsAny<int>())).Returns((int id) =>
                tasks.Where(x => x.AssigneeId == id));

            mockHumanTaskRepository.Setup(mr => mr.GetAllForEmployee(It.IsAny<int>())).Returns((int id) =>
                tasks.Where(x => x.CreatorId == id));

            mockHumanTaskRepository.Setup(mr => mr.GetById(It.IsAny<int>())).Returns((int id) =>
                tasks.Where(x => x.Id == id).Single());


            mockHumanTaskRepository.Setup(mr => mr.Add(It.IsAny<HumanTask>())).Returns((HumanTask target) =>
            {
                target.Id = tasks.Count() + 1;
                tasks.Add(target);
                return target;
            });*/
        }

        [Test]
        public void Should_AddTask()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);
           
            var testTask = new HumanTask{Id = 4, Name = "Fourth Task"};

            processor.CreateTask(testTask);
            mockHumanTaskRepository.Verify(it => it.Add(testTask), Times.Once());
        }
        [Test]
        public void Should_AddTaskWithReminder()
        {

        }
        [Test]
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
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);
            
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
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);
            
            processor.DeleteTask(1);
            mockHumanTaskRepository.Verify(it => it.Delete(1), Times.Once());
        }
        [Test]
        public void Should_ReturnListOfTasksOfEmployeeByHisId()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);

            processor.GetTaskById(1);
            mockHumanTaskRepository.Verify(it => it.GetById(1), Times.Once());
        }

        public void Should_ReturnListOfTasksOfAllTasks()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);

            processor.GetTaskById(1);
            mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }

        [Test]
        public void Should_ReturnListOfNotassignedTasks()
        {
            TaskProcessor processor = new TaskProcessor(mockHumanTaskRepository.Object);

            processor.GetTasksList();
            mockHumanTaskRepository.Verify(it => it.GetAll(), Times.Once());
        }
    }
}
