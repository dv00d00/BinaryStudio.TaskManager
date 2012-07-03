using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public interface IHumanTaskRepository
    {
        IEnumerable<HumanTask> GetForCreator(int creatorId);

        IList<HumanTask> GetAllTasksForEmployee(int employeeId);

        IList<HumanTask> GetUnassingnedTasks(); 
        
        IEnumerable<HumanTask> GetAll();

        HumanTask GetById(int humanTaskId);

        //TODO: refactor - why method return the same task, witch is in parameters??
        HumanTask Add(HumanTask humanTask);

        void Delete(int humanTaskId);

        void Update(HumanTask humanTask);
    }
}