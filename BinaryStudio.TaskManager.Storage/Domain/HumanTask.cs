using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class HumanTask : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Assigned { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? Closed { get; set; }

        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }

        public virtual Employee Creator { get; set; }

        [ForeignKey("Assignee")]
        public int? AssigneeId { get; set; }
        
        public virtual Employee Assignee { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}