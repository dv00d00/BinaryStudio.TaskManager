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
    using Ninject.Web.Common;

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
            this.Bind<DataBaseContext>().ToMethod(it => new DataBaseContext()).InRequestScope();
            this.Bind<ITaskProcessor>().To<TaskProcessor>().InRequestScope();
            this.Bind<IUserProcessor>().To<UserProcessor>().InRequestScope();
            this.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            this.Bind<IHumanTaskRepository>().To<HumanTaskRepository>().InRequestScope();
            this.Bind<IReminderRepository>().To<ReminderRepository>().InRequestScope();
            this.Bind<IProjectRepository>().To<ProjectRepository>().InRequestScope();
            this.Bind<ICryptoProvider>().To<CryptoProvider>().InRequestScope();
            this.Bind<IProjectProcessor>().To<ProjectProcessor>().InRequestScope();
            this.Bind<INotifier>().To<Notifier>().InRequestScope();
            this.Bind<IConnectionProvider>().To<ConnectionProvider>().InSingletonScope();
            this.Bind<IGlobalHost>().To<GlobalHostImpl>().InSingletonScope();
            this.Bind<INewsRepository>().To<NewsRepository>().InRequestScope();
        }
    }
}