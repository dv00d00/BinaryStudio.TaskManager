using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.NinjectModules
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Tests;

    using Ninject.Modules;

    /// <summary>
    /// Registers types in NInject IoC container
    /// </summary>
    public class MainInjectModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Bind<DataBaseContext>().ToMethod(it => new DataBaseContext());
            this.Bind<ITaskProcessor>().To<TaskProcessor>();
            this.Bind<IEmployeeRepository>().To<EmployeeRepository>();

           // this.Bind<IHumanTaskRepository>().To<HumanTaskRepository>();
            this.Bind<IHumanTaskRepository>().To<HumanTaskRepository>();
            this.Bind<IReminderRepository>().To<ReminderRepository>();

        }
    }
}