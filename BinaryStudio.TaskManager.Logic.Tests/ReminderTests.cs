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

            notifier.Verify(it => it.Send(It.IsAny<Guid>(), content));
        }

        [Test]
        public void Reminder_ShouldHave_AssignedEmployee()
        {
            throw new NotImplementedException();
        }
        
        [Test]
        public void Reminder_ShouldBeSend_ToSingleEmployee()
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    // most probably integration
    public class ReminderRepositoryTest
    {
        [Test]
        public void ShouldNot_ReturnRemindersFromFuture()
        {
            
        }

        [Test]
        public void Should_ReturnNotDeliveredRemindersFromThePast()
        {
            
        }

        // mmmmmore facts
    }

    public abstract class TimeManager
    {
        public event EventHandler<TimeArguments> OnTick;

        protected void RaiseTime(DateTime reminderDate)
        {
            if (this.OnTick != null)
            {
                this.OnTick(this, new TimeArguments
                                      {
                                          DateTime = reminderDate
                                      });
            }
        }
    }

    /// <summary>
    /// Fake time manager. Risese events on method call.
    /// </summary>
    public class MockTimeManager : TimeManager
    {
        /// <summary>
        /// Sends the time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        public void SendTime(DateTime dateTime)
        {
            this.RaiseTime(dateTime);
        }
    }

    public class TimeArguments : EventArgs
    {
        public DateTime DateTime { get; set; }
    }

    public interface IReminderRepository
    {
        IEnumerable<Reminder> GetReminderList(DateTime dateTime);
    }

    public class ReminderSender
    {
        private readonly TimeManager timeManager;
        private readonly INotifier notifier;
        private readonly IReminderRepository reminderRepository;

        public ReminderSender(TimeManager timeManager, INotifier notifier, IReminderRepository reminderRepository)
        {
            this.timeManager = timeManager;
            this.notifier = notifier;
            this.reminderRepository = reminderRepository;

            this.timeManager.OnTick += (s, e) =>
            {
                var reminders = this.reminderRepository.GetReminderList(e.DateTime);

                foreach (var reminder in reminders)
                {
                    this.notifier.Send(new Guid(), reminder.Content);
                }
            };
        }
    }
}