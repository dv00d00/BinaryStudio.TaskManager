using System;
using System.Timers;
namespace BinaryStudio.TaskManager.Logic.Core
{
    public class ReminderSender : IReminderSender
    {
        private readonly Timer timer;
        private readonly INotifier notifier;
        private readonly IReminderProcessor reminderProcessor;


        public ReminderSender(INotifier notifier,IReminderProcessor reminderProcessor)
        {
            this.notifier = notifier;
            this.timer = new Timer(300000); // 5 minutes
            this.timer.Elapsed += new ElapsedEventHandler(OnTick);
            this.reminderProcessor = reminderProcessor;
            //this.timeManager.OnTick += this.OnTick;
        }

        public void StartTimer()
        {
            this.timer.Start();
        }

        public void StopTimer()
        {
            this.timer.Stop();
        }

        public void OnTick(object s, ElapsedEventArgs e)
        {
            var reminders = this.reminderProcessor.GetRemindersOnDate(DateTime.Now);

           foreach(var reminder in reminders)
           {
               if(notifier.SendReminderToDesktopClient(reminder.UserId,reminder.Content))
               {
                   reminder.IsSend = true;
                   reminderProcessor.UpdateReminder(reminder);
               }
           }
            //foreach (var reminder in reminders)
            //{
            //    var clientConnection = this.clientConnectionManager.GetClientByEmployeeId(reminder.User.Id);
            //    if (clientConnection != null)
            //    {
            //        this.notifier.Send(clientConnection, reminder.Content);
            //    }
            //}
        }
    }
}