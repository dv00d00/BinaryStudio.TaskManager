﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProjectViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Models
{
    using System.Collections.Generic;

    using BinaryStudio.TaskManager.Logic.Domain;

    public class ProjectViewModel
    {
        public List<ManagerTasksViewModel> UsersTasks { get; set; }

        public List<HumanTask> UnAssignedTasks { get; set; } 
    }
}