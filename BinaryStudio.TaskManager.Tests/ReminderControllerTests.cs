using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Web.Tests
{
    using System.Web.Mvc;

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
        private Mock<IEmployeeRepository> mockEmployeeRepository;

        private Reminder reminder;

        private const int taskId = 1; 

        [SetUp]
        public void SetUp()
        {
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockTaskProcessor = new Mock<ITaskProcessor>();
            mockReminderRepository = new Mock<IReminderRepository>();

            reminder = new Reminder { Id = taskId, Content = "asd", EmployeeID = 1 };

            mockReminderRepository.Setup(x => x.GetById(taskId)).Returns(reminder);
        }
        
        [Test]
        public void Should_ReturnIndexView_WhenDefaultIndexIsCalled()
        {
            // arrange
            var reminderController = new ReminderController(mockReminderRepository.Object,
               mockEmployeeRepository.Object, mockTaskProcessor.Object);

            // act
            var result = reminderController.Index();
            
            // assert
            result.AssertViewRendered().ForView("");
        }

        [Test]
        public void Should_ReturnDetailsViewWithReminder_WhenDetailsViewIsCalled()
        {
            //arrange
            var reminderController = new ReminderController(mockReminderRepository.Object,
               mockEmployeeRepository.Object, mockTaskProcessor.Object);
            
            //act
            var result = reminderController.Details(taskId);

            //assert
            result.AssertViewRendered().ForView("").WithViewData<Reminder>();
        }

        [Test]
        public void Should_ReturnCreateView_WhenCreateIsCalled()
        {
            //arrange
            var reminderController = new ReminderController(mockReminderRepository.Object,
                mockEmployeeRepository.Object, mockTaskProcessor.Object);

            //act
            var result = reminderController.Create();

            //assert
            result.AssertViewRendered().ForView("");
        }

     //[Test]
     //public void Should_ReturnCreateViewWithReminderData_WhenCreateWithParameterIsCalled()
     //{
     //    //arrange
     //    var reminderController = new ReminderController(mockReminderRepository.Object,
     //        mockEmployeeRepository.Object, mockTaskProcessor.Object);
     //    
     //    //act
     //    var result = reminderController.Create(reminderWithNoEmployee);
     //
     //    //assert
     //    result.AssertViewRendered().ForView("").WithViewData<Reminder>();
     //}

        [Test]
        public void Should_RedirectToIndex_WhenReminderBeingAddedIsCorrect()
        {
            // arrange
            var reminderController = new ReminderController(mockReminderRepository.Object,
               mockEmployeeRepository.Object, mockTaskProcessor.Object);
            var reminder = new Reminder { Id = 1, Content = "asd", EmployeeID = 1 };
            
            //act
            var result = reminderController.Create(reminder);

            //assert
            Assert.IsNotNull(result);
            result.AssertActionRedirect().ToAction("Index");
        }

        [Test]
        public void Should_ReturnViewWithRemiderData_WhenEditWithIDIsCalled()
        {
            //arrange
            var reminderController = new ReminderController(mockReminderRepository.Object, 
                mockEmployeeRepository.Object, mockTaskProcessor.Object);
           
            //act
            var result = reminderController.Edit(taskId);

            //assert
            Assert.IsNotNull(result);
            result.AssertViewRendered().ForView("").WithViewData<Reminder>();
        }

        [Test]
        public void Should_ReturnEditViewWithRemiderData_WhenEditWithReminderIsCalled()
        {
            //arrange
            var reminderController = new ReminderController(mockReminderRepository.Object,
                mockEmployeeRepository.Object, mockTaskProcessor.Object);

            //act
            var result = reminderController.Edit(reminder);

            //assert
            Assert.IsNotNull(result);
            result.AssertActionRedirect().ToAction("Index");
        }

        [Test]
        public void Should_ReturnDeleteViewWithRemiderData_WhenDeleteIsCalled()
        {
            //arrange
            var reminderController = new ReminderController(mockReminderRepository.Object,
                mockEmployeeRepository.Object, mockTaskProcessor.Object);

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
            var reminderController = new ReminderController(mockReminderRepository.Object,
                mockEmployeeRepository.Object, mockTaskProcessor.Object);
            
            //act
            var result = reminderController.DeleteConfirmed(taskId);

            //assert
            Assert.IsNotNull(result);
            result.AssertActionRedirect().ToAction("Index");
        }

    }
}
