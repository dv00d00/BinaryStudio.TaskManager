using System.Collections.Generic;
using System.Web.Mvc;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class SingleTaskViewModel
    {
        public IEnumerable<SelectListItem> Priorities { get; set; }

        public HumanTask HumanTask { get; set; }

        public string TaskName { get; set; }

        public string CreatorName { get; set; }
        
        public string AssigneeName { get; set; }

        public string BlockedTaskName { get; set; }

        public bool? ViewStyle { get; set; }

        public IList<HumanTaskHistory> TaskHistories { get; set; }

    }
}