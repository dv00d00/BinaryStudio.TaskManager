using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IReminderProcessor
    {
        void AddReminder(Reminder reminder);
        void DeleteReminder(int reminderId);
        void UpdateReminder(Reminder reminder);
    }
}
