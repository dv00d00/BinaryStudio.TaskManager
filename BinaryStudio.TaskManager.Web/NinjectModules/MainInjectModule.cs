namespace BinaryStudio.TaskManager.Web.NinjectModules
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Storage;

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
            this.Bind<TaskContext>().ToMethod(it => new TaskContext());
            this.Bind<IHumanTaskRepository>().To<HumanTaskRepository>();
        }
    }
}