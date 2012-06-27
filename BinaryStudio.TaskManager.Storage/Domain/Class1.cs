using System;
using System.Collections.Generic;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Task
    {
        public string Description { get; set; }

        public Reminder Current { get; set; }

        public List<Reminder> PreviousReminders { get; set; }
         
    }

    public class Reminder
    {
        public DateTime ReminderDate { get; set; }

        public string Content { get; set; }

        public bool WasDelivered { get; set; }
    }
}