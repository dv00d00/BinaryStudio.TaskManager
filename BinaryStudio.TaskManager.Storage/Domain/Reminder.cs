using System;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Reminder
    {
        /// <summary>
        /// Reference to asociated employee, should not be 0
        /// </summary>
        public int EmployeeID { get; set; }

        /// <summary>
        /// Reference to associated task. If null remandier had no associated task.
        /// </summary>
        public int? TaskId { get; set; }

        public DateTime ReminderDate { get; set; }

        public string Content { get; set; }

        public bool WasDeliviered { get; set; }

        public Employee Employee { get; set; }
    }
}