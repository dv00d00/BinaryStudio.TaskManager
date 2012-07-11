using System;
using System.Collections.Generic;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Project : IEntity
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<HumanTask> Tasks { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual User Creator { get; set; }

        public int CreatorId { get; set; }
    }
}
