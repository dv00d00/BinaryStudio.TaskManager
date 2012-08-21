using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class ReminderProcessor : IReminderProcessor
    {
        private readonly IReminderRepository reminderRepository;

        public ReminderProcessor(IReminderRepository reminderRepository)
        {
            this.reminderRepository = reminderRepository;
        }

        public void AddReminder(Reminder reminder)
        {
            reminderRepository.Add(reminder);
        }

        public void DeleteReminder(int reminderId)
        {
            reminderRepository.Delete(reminderId);
        }

        public void UpdateReminder(Reminder reminder)
        {
            reminderRepository.Update(reminder);
        }

        public IList<Reminder> GetRemindersForUser(int userId)
        {
            IEnumerable<Reminder> reminders = reminderRepository.GetAll();
            var resreminders = reminders.Where(x => x.UserId == userId && x.WasDelivered == false
                                             && x.ReminderDate <= DateTime.Now).
                ToList();

            return resreminders;
        }

        public bool IsUserHaveReminders(int userId)
        {
            return GetRemindersForUser(userId).Count > 0;
        }

        public IList<Reminder> GetRemindersOnDate(DateTime dateTime)
        {
            IList<Reminder> reminders = reminderRepository.GetAll().ToList();
            return reminders.Where(x => x.WasDelivered == false && x.IsSend==false
                && x.ReminderDate <= dateTime).ToList();
        }

        public IList<Reminder> GetRemindersOnDateForSender(DateTime dateTime)
        {
            IList<Reminder> reminders = reminderRepository.GetAllForSender().ToList();
            return reminders.Where(x => x.WasDelivered == false && x.IsSend == true
                && x.ReminderDate <= dateTime).ToList();
        }

        public void DeleteRemindersForTask(int taskId)
        {
            var reminders = this.reminderRepository.GetAll().Where(x => x.TaskId.HasValue && x.TaskId.Value == taskId).ToList();
            if (reminders != null)
            {
                foreach (var reminder in reminders)
                {
                    this.reminderRepository.Delete(reminder);
                }
            }
        }

        public IEnumerable<Reminder> GetAll()
        {
            return this.reminderRepository.GetAll();
        }

        public IEnumerable<Reminder> GetRemindersForTask(int taskId)
        {
            return this.reminderRepository.GetAll().Where(x => x.TaskId.HasValue && x.TaskId.Value == taskId);
        }
    }
}
