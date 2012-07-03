using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class TaskViewModel
    {
        
        public HumanTask Task { get; set; }

        public string CreatorName { get; set; }
    }
}