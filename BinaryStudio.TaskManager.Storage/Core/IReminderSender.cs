using System.Timers;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IReminderSender
    {
        void StartTimer();
        void StopTimer();
        void OnTick(object s, ElapsedEventArgs e);
    }
}
