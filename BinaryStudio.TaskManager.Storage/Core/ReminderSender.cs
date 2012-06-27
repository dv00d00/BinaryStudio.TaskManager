namespace BinaryStudio.TaskManager.Logic.Core
{
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
                                                   this.notifier.Send(new ClientConnection(), reminder.Content);
                                               }
                                           };
        }
    }
}