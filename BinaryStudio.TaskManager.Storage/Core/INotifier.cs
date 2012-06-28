namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;

    public interface INotifier
    {
        void Broadcast(string message);

        void Send(ClientConnection connectionId, string message);
    }

    public class ClientConnection
    {
        public int? EmployeeID { get; set; }
        public Guid ConnectionID { get; set; }

    }
}