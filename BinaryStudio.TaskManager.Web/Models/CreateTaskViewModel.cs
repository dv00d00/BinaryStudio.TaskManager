// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTaskViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CreateTaskViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// The create task view model.
    /// </summary>
    public class CreateTaskViewModel
    {
        public IEnumerable<SelectListItem> Priorities;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Assigned { get; set; }

        public DateTime? Finished { get; set; }

        public DateTime? Closed { get; set; }

        public int? CreatorId { get; set; }

        public int? AssigneeId { get; set; }

        public int ProjectId { get; set; }

        public bool IsBlocking { get; set; }

        public IEnumerable<SelectListItem> Tasks;

        public int BlockingTask { get; set; }
    }
}