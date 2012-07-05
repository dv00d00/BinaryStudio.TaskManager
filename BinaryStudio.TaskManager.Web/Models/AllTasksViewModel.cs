using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class AllTasksViewModel
    {
        public HumanTask HumanTask { get; set; }

        public string CreatorName { get; set; }
        
        public string AssigneeName { get; set; }

    }
}