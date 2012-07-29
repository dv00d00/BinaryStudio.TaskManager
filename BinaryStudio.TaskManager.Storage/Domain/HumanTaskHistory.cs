using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class HumanTaskHistory : IEntity
    {
        public int Id { get; set; }

        public String NewName { get; set; }

        public String NewDescription { get; set; }

        public DateTime ChangeDateTime { get; set; }

        public int? NewAssigneeId { get; set; }

        public int TaskId { get; set; }

        public virtual HumanTask Task { get; set; }

        public int NewPriority { get; set; }
    }
}
