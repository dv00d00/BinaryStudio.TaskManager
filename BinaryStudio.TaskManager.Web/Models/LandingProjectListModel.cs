namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class LandingProjectListModel
    {
        public IEnumerable<LandingProjectModel> UserProjects { get; set; }

        public IEnumerable<LandingProjectModel> CreatorProjects { get; set; }
    }
}