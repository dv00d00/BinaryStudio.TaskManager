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
    }
}
