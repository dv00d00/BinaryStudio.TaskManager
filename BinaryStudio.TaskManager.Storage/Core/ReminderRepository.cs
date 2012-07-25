using System.Data;
using System.Linq;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ReminderRepository : IReminderRepository
    {

         private readonly DataBaseContext dataBaseContext;

         public ReminderRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }


        public void Add(Reminder reminder)
        {
            this.dataBaseContext.Entry(reminder).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public void Delete(Reminder reminder)
        {
            this.dataBaseContext.Reminders.Remove(reminder);
            this.dataBaseContext.SaveChanges();
        }

        public void Update(Reminder reminder)
        {
            this.dataBaseContext.Entry(reminder).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        public Reminder GetById(int reminderId)
        {
            return this.dataBaseContext.Reminders.Single(reminder => reminder.Id == reminderId);
        }

        public IEnumerable<Reminder> Get(Func<Reminder> selector)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reminder> GetReminderList(DateTime dateTime)
        {
            return this.dataBaseContext.Reminders.Where(reminder => reminder.ReminderDate == dateTime).ToList();
        }

        public IEnumerable<Reminder> GetAll()
        {
            return this.dataBaseContext.Reminders.ToList();
        }

        public IEnumerable<Reminder> GetAllRemindersForUser(int userId)
        {
            return this.dataBaseContext.Reminders.Where(it => it.User.Id == userId);
        }
    }
}