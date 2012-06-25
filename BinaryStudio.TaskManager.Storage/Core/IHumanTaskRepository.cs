using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public interface IHumanTaskRepository
    {
        IEnumerable<HumanTask> GetForCreator(int creatorId);

        IEnumerable<HumanTask> GetAllForEmployee(int employeeId);

        IEnumerable<HumanTask> GetAll();

        HumanTask GetById(int humanTaskId);

        HumanTask Add(HumanTask humanTask);

        void Delete(int id);

        void Update(HumanTask humanTask);
    }
}