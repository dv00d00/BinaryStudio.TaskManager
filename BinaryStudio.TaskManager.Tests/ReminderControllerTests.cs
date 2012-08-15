using NUnit.Framework;

namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using MvcContrib.TestHelper;

    [TestFixture]
    class ReminderControllerTests
    {
        private Mock<ITaskProcessor> mockTaskProcessor;
        private Mock<IReminderRepository> mockReminderRepository;
        private Mock<IUserRepository> mockUserRepository;

        private Reminder reminder;
        private Mock<IReminderProcessor> mockReminderProcessor; 
        private ReminderController reminderController;

        private const int taskId = 1; 

        [SetUp]
        public void SetUp()
        {
            this.mockUserRepository = new Mock<IUserRepository>();
            this.mockTaskProcessor = new Mock<ITaskProcessor>();
            this.mockReminderRepository = new Mock<IReminderRepository>();
            this.mockReminderProcessor = new Mock<IReminderProcessor>();

            this.reminderController = new ReminderController(
                this.mockReminderRepository.Object,
                this.mockUserRepository.Object, 
                this.mockTaskProcessor.Object,
                mockReminderProcessor.Object);

            this.reminder = new Reminder { Id = taskId, Content = "asd", UserId = 1 };
            
            this.mockReminderRepository.Setup(x => x.GetById(taskId)).Returns(reminder);
        }
        
        [Test]
        public void Should_ReturnDetailsViewWithReminder_WhenDetailsViewIsCalled()
        {
            //arrange
            
            //act
            var result = reminderController.Details(taskId);

            //assert
            result.AssertViewRendered().ForView("").WithViewData<Reminder>();
        }

        [Test]
        public void Should_ReturnViewWithRemiderData_WhenEditWithIDIsCalled()
        {
            //arrange            
           
            //act
            var result = reminderController.Edit(taskId);

            //assert
            Assert.IsNotNull(result);
            result.AssertViewRendered().ForView("").WithViewData<Reminder>();
        }

        [Test]
        public void Should_ReturnDeleteViewWithRemiderData_WhenDeleteIsCalled()
        {
            //arrange

            //act
            var result = reminderController.Delete(taskId);

            //assert
            Assert.IsNotNull(result);
            result.AssertViewRendered().ForView("");
        }

        [Test]
        public void Should_RedirectToIndex_WhenDeleteConfirmated()
        {
            //arrange
            
            //act
            var result = reminderController.DeleteConfirmed(taskId);

            //assert
            Assert.IsNotNull(result);
            result.AssertActionRedirect().ToAction("Index");
        }

    }
}
