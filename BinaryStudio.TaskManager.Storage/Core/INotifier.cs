namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;

    public interface INotifier
    {
        void Broadcast(string message);

        void Send(Guid connectionId, string message);
    }
}