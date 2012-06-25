using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using BinaryAcademia.AllManagerView.Models;
using BinaryStudio.TaskManager.Web.Models;

namespace BinaryStudio.TaskManager.Web.Content
{
    //Generate test data for AllManagersView
    public class AllManagersDataTest
    {
        public ICollection<ManagerModel> managers;
        private ICollection<TaskModel> _tasks;
        public AllManagersDataTest()
        {

        }

        public ICollection<ManagerModel> GetManagers()
        {
            ICollection<ManagerModel> managers = new Collection<ManagerModel>();
            ManagerModel tempManager = new ManagerModel(1, "Vova Pupkin");
            tempManager.Tasks = new Collection<TaskModel>();
            TaskModel tempTask = new TaskModel(1, "Super Title", "Super descsription");
            tempManager.Tasks.Add(tempTask);
            tempTask = new TaskModel(2, "Its simple!", "Just try add some tasks!");
            tempManager.Tasks.Add(tempTask);
            tempTask = new TaskModel(3, "Oh no!", "Something was broken!");
            tempManager.Tasks.Add(tempTask);
            managers.Add(tempManager);

            tempManager = new ManagerModel(2, "Olololo");
            tempManager.Tasks = new Collection<TaskModel>();
            managers.Add(tempManager);

            tempManager = new ManagerModel(3, "IronMan");
            tempManager.Tasks = new Collection<TaskModel>();
            tempTask = new TaskModel(4, "Shaurma time!", "Lets eat some shaurma. P.S. Tor'll get it after saving the World");
            tempManager.Tasks.Add(tempTask);
            managers.Add(tempManager);
            return managers;
        }

    }
}