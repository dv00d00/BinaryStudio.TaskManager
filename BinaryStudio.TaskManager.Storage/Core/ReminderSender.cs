namespace BinaryStudio.TaskManager.Logic.Core
{
    public class ReminderSender
    {
        private readonly TimeManager timeManager;
        private readonly INotifier notifier;
        private readonly IReminderRepository reminderRepository;
        private readonly IClientConnectionManager clientConnectionManager;

        public ReminderSender(TimeManager timeManager, INotifier notifier, IReminderRepository reminderRepository,
                              IClientConnectionManager clientConnectionManager)
        {
            this.timeManager = timeManager;
            this.notifier = notifier;
            this.reminderRepository = reminderRepository;
            this.clientConnectionManager = clientConnectionManager;

            this.timeManager.OnTick += this.OnTick;
        }

        private void OnTick(object s, TimeArguments e)
        {
            var reminders = this.reminderRepository.GetReminderList(e.DateTime);

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