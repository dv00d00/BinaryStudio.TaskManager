using System;
using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IReminderProcessor
    {
        void AddReminder(Reminder reminder);
        void DeleteReminder(int reminderId);
        void UpdateReminder(Reminder reminder);
        IList<Reminder> GetRemindersForUser(int userId);
        bool IsUserHaveReminders(int userId);
        IList<Reminder> GetRemindersOnDate(DateTime dateTime);
        IList<Reminder> GetRemindersOnDateForSender(DateTime dateTime);
        void DeleteRemindersForTask(int taskId);
        IEnumerable<Reminder> GetAll();
    }
}
