using System;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public abstract class TimeManager
    {
        public event EventHandler<TimeArguments> OnTick;

        protected void RaiseTime(DateTime reminderDate)
        {
            if (this.OnTick != null)
            {
                this.OnTick(this, new TimeArguments
                                      {
                                          DateTime = reminderDate
                                      });
            }
        }
    }
}