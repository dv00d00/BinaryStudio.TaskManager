using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
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
    }
}