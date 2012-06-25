using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BinaryAcademia.AllManagerView.Models;

namespace BinaryStudio.TaskManager.Web.Models
{
    //Must be fixed 
    public class TaskModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ManagerModel Manager { get; set; }

        public TaskModel(
            int id,
            string title,
            string description)
        {
            TaskId = id;
            Title = title;
            Description = description;
        }

        public TaskModel(
            int id,
            string title,
            string description,
            ManagerModel manager)
        {
            TaskId = id;
            Title = title;
            Description = description;
            Manager = manager;
        }


    }
}