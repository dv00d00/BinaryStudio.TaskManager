using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class ListEventViewModel
    {
        public List<EventViewModel> Events { get; set; }
        public List<ProjectDataForEventsViewModel> Projects { get; set; } 
    }
}