using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class ManagersViewModel
    {
        public List<ManagerTasksViewModel> ManagerTasks { get; set; }
        public List<HumanTask> UnAssignedTasks { get; set; } 
    }
}