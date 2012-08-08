using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class InvintationsInEventsViewModel
    {
        public int SenderId { get; set; }

        public string SenderName { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int InvitationId { get; set; }
    }
}