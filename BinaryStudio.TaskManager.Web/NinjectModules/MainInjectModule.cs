// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainInjectModule.cs" company="">
//   
// </copyright>
// <summary>
//   Registers types in NInject IoC container
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.NinjectModules
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Logic.Domain;

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
            this.Bind<IUserProcessor>().To<UserProcessor>();            
            this.Bind<IUserRepository>().To<UserRepository>();
            this.Bind<IHumanTaskRepository>().To<HumanTaskRepository>();
            this.Bind<IReminderRepository>().To<ReminderRepository>();
            this.Bind<IProjectRepository>().To<ProjectRepository>();
            this.Bind<ICryptoProvider>().To<CryptoProvider>();
        }
    }
}