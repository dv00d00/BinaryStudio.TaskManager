using System;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Reminder : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }

        [ForeignKey("Task")]
        public int? TaskId { get; set; }

        public virtual HumanTask Task { get; set; }

        public DateTime ReminderDate { get; set; }

        public string Content { get; set; }

        public bool WasDelivered { get; set; }
    }
}