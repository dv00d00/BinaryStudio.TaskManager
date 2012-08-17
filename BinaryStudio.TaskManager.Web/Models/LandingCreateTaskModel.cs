namespace BinaryStudio.TaskManager.Web.Models
{
    public class LandingCreateTaskModel
    {
        public string Name { get; set; }

        public int Priority { get; set; }

        public int? AssigneeId { get; set; }

        public int ProjectId { get; set; }
    }
}