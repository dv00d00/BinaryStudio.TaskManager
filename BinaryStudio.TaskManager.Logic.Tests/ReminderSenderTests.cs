using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using Moq;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Logic.Tests
{
    [TestFixture]
    public class ReminderSenderTests
    {
        [Test]
        public void Should_SendNotification_When_TimeHasCome()
        {
            var reminderDate = new DateTime(2010, 11, 10, 10, 0, 0);

            var timeManager = new MockTimeManager();
            var notifier = new Mock<INotifier>();
            var reminderRepository = new Mock<IReminderRepository>();

            const string content = "asdkjasdnajkn";
            reminderRepository.Setup(it => it.GetReminderList(reminderDate)).Returns(new List<Reminder> {new Reminder()
                {
                    Content = content
                }});

            new ReminderSender(timeManager, notifier.Object, reminderRepository.Object);

            timeManager.SendTime(reminderDate);

            notifier.Verify(it => it.Send(It.IsAny<ClientConnection>(), content));
        }

        [Test]
        public void ShouldNot_SendNotification_When_EmployeeIsNotAssigned()
        {
            var reminderDate = new DateTime(2010, 11, 10, 10, 0, 0);

            var timeManager = new MockTimeManager();
            var notifier = new Mock<INotifier>();
            var reminderRepository = new Mock<IReminderRepository>();

            const string content = "asdkjasdnajkn";
            reminderRepository.Setup(it => it.GetReminderList(reminderDate)).Returns(new List<Reminder> {new Reminder()
                {
                    Content = content,
                    Employee = null
                }});

            new ReminderSender(timeManager, notifier.Object, reminderRepository.Object);

            timeManager.SendTime(reminderDate);

            notifier.Verify(it => it.Send(It.IsAny<ClientConnection>(), content));
        }
        
        [Test]
        public void Reminder_ShouldBeSend_ToSingleEmployee()
        {
            var reminderDate = new DateTime(2012, 11, 10, 10, 0, 0);

            var timeManager = new MockTimeManager();
            var notifier = new Mock<INotifier>();
            var reminderRepository = new Mock<IReminderRepository>();

            const string content = "lsdkjfklsjd";
            const int employeeID = 123123;
            reminderRepository.Setup(it => it.GetReminderList(reminderDate)).Returns(new List<Reminder>
            {
                new Reminder()
                    {
                        Content = content,
                        EmployeeID = employeeID
                    }
            });
            new ReminderSender(timeManager, notifier.Object, reminderRepository.Object);

            timeManager.SendTime(reminderDate);

            notifier.Verify(it => it.Send(It.IsAny<ClientConnection>(), content));
        }
    }
}