using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryAcademia.AllManagerView.Models
{
    //Must be fixed 
    public class ManagerModel
    {
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TaskModel> Tasks { get; set; }

        public ManagerModel(int managerId, string name)
        {
            ManagerId = managerId;
            Name = name;
        }
    }
}