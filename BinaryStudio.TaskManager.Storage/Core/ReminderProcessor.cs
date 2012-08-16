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
            return reminderRepository.GetAll().Where(x => x.UserId == userId && x.WasDelivered == false 
                && x.ReminderDate.ToShortDateString()==DateTime.Now.ToShortDateString()).ToList();
        }

        public bool IsUserHaveReminders(int userId)
        {
            return GetRemindersForUser(userId).Count > 0;
        }

        public IList<Reminder> GetRemindersOnDate(DateTime dateTime)
        {
            return reminderRepository.GetAll().Where(x => x.WasDelivered == false && x.IsSend==false
                && x.ReminderDate.ToShortDateString() == dateTime.ToShortDateString()).ToList();
        }
    }
}
