using System;
using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IReminderRepository
    {
        void Add(Reminder reminder);
        void Delete(Reminder reminder);
        void Delete(int reminderId);
        void Update(Reminder reminder);

        Reminder GetById(int reminderId);
        IEnumerable<Reminder> Get(Func<Reminder> selector);
        IEnumerable<Reminder> GetReminderList(DateTime dateTime);
        IEnumerable<Reminder> GetAll();
        IEnumerable<Reminder> GetAllRemindersForUser(int userId);
    }
}