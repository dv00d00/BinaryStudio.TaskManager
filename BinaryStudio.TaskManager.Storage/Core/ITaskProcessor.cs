using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{

    public interface ITaskProcessor
    {
        void CreateTask(HumanTask task);
        void CreateTask(HumanTask task, Reminder reminder);

        void UpdateTask(HumanTask task);
        void UpdateTask(HumanTask task, Reminder reminder);

        void DeleteTask(int taskId);

        void MoveTask(int taskId, int employeeId);
        void MoveTaskToUnassigned(int taskId);
        
        void AssignTask(int taskId, int emploeeyId);

        void CloseTask(int taskId);

        HumanTask GetTaskById(int taskId);
        IEnumerable<HumanTask> GetUnassignedTasks();
        IEnumerable<HumanTask> GetTasksList(int employeeId);
        
    }
}
