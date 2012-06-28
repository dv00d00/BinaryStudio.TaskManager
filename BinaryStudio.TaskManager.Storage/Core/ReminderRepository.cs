namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ReminderRepository : IReminderRepository
    {
        public void Add(Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public void Delete(Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public void Update(Reminder reminder)
        {
            throw new NotImplementedException();
        }

        public Reminder GetById(int reminderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reminder> Get(Func<Reminder> selector)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reminder> GetReminderList(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reminder> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}