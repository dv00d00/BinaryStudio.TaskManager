namespace BinaryStudio.TaskManager.Web.Models
{
    public class EventViewModel
    {
        public int NewsId { get; set; }

        public string ProjectName { get; set; }

        public int ProjectId { get; set; }

        public string TaskName { get; set; }

        public int TaskId { get; set; }

        public string Action { get; set; }

        public string WhoChangeUserName { get; set; }

        public int WhoChangeUserId { get; set; }

        public string WhoAssigneUserName { get; set; }

        public int? WhoAssigneUserId { get; set; }

        public string Details { get; set; }

        public string TimeAgo { get; set; }

        public bool IsRead { get; set; }

        public string ContainerClassName { get; set; }

        public string WhoChangeLinkDetails { get; set; }

        public string WhoAssigneLinkDetails { get; set; }

        public string TaskLinkDetails { get; set; }

        public bool IsMove { get; set; }

        public bool IsAssigne { get; set; }

    }
}
