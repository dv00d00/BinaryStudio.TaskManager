using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public interface IHumanTaskRepository
    {
        IEnumerable<HumanTask> GetForCreator(int creatorId);

        IList<HumanTask> GetAllTasksForEmployee(int employeeId);

        IEnumerable<HumanTask> GetAll();

        HumanTask GetById(int humanTaskId);

        HumanTask Add(HumanTask humanTask);

        void Delete(int humanTaskId);

        void Update(HumanTask humanTask);
    }
}