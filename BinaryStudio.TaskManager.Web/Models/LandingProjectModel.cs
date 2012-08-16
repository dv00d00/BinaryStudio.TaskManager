namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Extensions;
    using BinaryStudio.TaskManager.Extensions.Models;
    using BinaryStudio.TaskManager.Logic.Domain;

    public class LandingProjectModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Creator { get; set; }

        public IEnumerable<LandingTaskModel> Tasks { get; set; }

        public IEnumerable<LandingUserModel> Users { get; set; }
    }
}