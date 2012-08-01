using System;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class ClientConnection
    {
        public String ConnectionId { get; set; }

        public string UserName { get; set; }

        public int? UserId { get; set; }

        public int? ProjectId { get; set; }

        public bool IsNewsConnection { get; set; }
    }
}