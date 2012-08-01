using System;
using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Logic.Tests
{
    [TestFixture]
    public class ReminderSenderTests
    {
        public MockTimeManager timeManager;
        private Mock<INotifier> notifier;
        private Mock<IReminderRepository> reminderRepository;
        private Mock<IClientConnectionManager> clientConnectionManager;


        [SetUp]
        public void Initialize()
        {
            timeManager = new MockTimeManager();
            notifier = new Mock<INotifier>();
            reminderRepository = new Mock<IReminderRepository>();
            clientConnectionManager = new Mock<IClientConnectionManager>();
        }

        //[Test]
        //public void Should_SendNotification_When_TimeHasCome()
        //{
        //    //arrange
        //    var reminderDate = new DateTime(2010, 11, 10, 10, 0, 0);
        //    const string content = "asdkjasdnajkn";
        //    reminderRepository.Setup(it => it.GetReminderList(reminderDate)).Returns(new List<Reminder> {new Reminder()
        //        {
        //            Content = content,
        //            UserId = 1
        //        }});
        //    clientConnectionManager.Setup(it => it.GetClientByEmployeeId(1)).Returns(new ClientConnection { EmployeeId = 1 });

        //    new ReminderSender(timeManager, notifier.Object, reminderRepository.Object, clientConnectionManager.Object);

        //    //act

        //    timeManager.SendTime(reminderDate);

        //    //assert

        //    notifier.Verify(it => it.Send(It.IsAny<ClientConnection>(), content), Times.AtLeastOnce());
        //}

        //[Test]
        //public void ShouldNot_SendNotification_WhenUserIsNotAssigned()
        //{
        //    // arrange
        //    var reminderDate = new DateTime(2010, 11, 10, 10, 0, 0);
        //    const string Content = "asdkjasdnajkn";
        //    this.reminderRepository.Setup(it => it.GetReminderList(reminderDate)).Returns(
        //        new List<Reminder> { new Reminder() { Content = Content, UserId = 1 } });
        //    this.clientConnectionManager.Setup(it => it.GetClientByEmployeeId(1));

        //    new ReminderSender(this.timeManager, this.notifier.Object, this.reminderRepository.Object, this.clientConnectionManager.Object);

        //    // act
        //    this.timeManager.SendTime(reminderDate);

        //    // assert
        //    this.notifier.Verify(it => it.Send(It.IsAny<ClientConnection>(), Content), Times.Never());
        //}

        //[Test]
        //public void Reminder_ShouldBeSend_ToSingleUser()
        //{
        //    // arrange
        //    var reminderDate = new DateTime(2012, 11, 10, 10, 0, 0);
        //    const string Content = "lsdkjfklsjd";
        //    var myClientConnection = new ClientConnection();
        //    this.reminderRepository.Setup(it => it.GetReminderList(reminderDate)).Returns(
        //        new List<Reminder>
        //            {
        //                new Reminder() { Content = Content, UserId = 1 },
        //                new Reminder { Content = "dddddd", UserId = 2 }
        //            });
        //    this.clientConnectionManager.Setup(it => it.GetClientByEmployeeId(1)).Returns(myClientConnection);
        //    new ReminderSender(this.timeManager, this.notifier.Object, this.reminderRepository.Object, this.clientConnectionManager.Object);

        //    // act
        //    this.timeManager.SendTime(reminderDate);

        //    // assert
        //    this.notifier.Verify(it => it.Send(myClientConnection, Content), Times.Once());
        //}
    }
}