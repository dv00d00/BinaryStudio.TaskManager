using System.Linq;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public interface IHumanTaskRepository
    {
        IEnumerable<HumanTask> GetForCreator(int creatorId);

        IList<HumanTask> GetAllTasksForEmployee(int employeeId);

        IList<HumanTask> GetAllTasksInProject(int projectId);

        IList<HumanTask> GetUnassingnedTasks(); 
        
        IEnumerable<HumanTask> GetAll();

        HumanTask GetById(int humanTaskId);

        //TODO: refactor - why method return the same task, witch is in parameters??
        void Add(HumanTask humanTask);

        void Delete(int humanTaskId);

        void Update(HumanTask humanTask);

        IList<HumanTaskHistory> GetAllHistoryForTask(int taskId);

        void AddHistory(HumanTaskHistory humanTaskHistory);

        IQueryable<Priority> GetPriorities();

        IList<HumanTask> GetUnassingnedTasks(int projectId);

        IEnumerable<HumanTask> GetAllTasksForUserInProject(int projectId, int userId);

        IList<HumanTaskHistory> GetAllHistoryForUser(int userId);
    }
}