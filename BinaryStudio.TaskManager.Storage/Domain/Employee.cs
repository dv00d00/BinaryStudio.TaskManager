using System.Collections.Generic;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<HumanTask> Tasks { get; set; }

        public int? UserId { get; set; }
    }
}