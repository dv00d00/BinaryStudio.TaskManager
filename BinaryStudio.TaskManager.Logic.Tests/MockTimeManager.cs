using System;
using BinaryStudio.TaskManager.Logic.Core;

namespace BinaryStudio.TaskManager.Logic.Tests
{
    /// <summary>
    /// Fake time manager. Risese events on method call.
    /// </summary>
    public class MockTimeManager : TimeManager
    {
        /// <summary>
        /// Sends the time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        public void SendTime(DateTime dateTime)
        {
            this.RaiseTime(dateTime);
        }
    }
}