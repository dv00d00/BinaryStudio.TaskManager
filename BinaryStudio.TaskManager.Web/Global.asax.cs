using System.Web.Routing;
using BinaryStudio.TaskManager.Logic.Core;
using SignalR;
using SignalR.Ninject;

namespace BinaryStudio.TaskManager.Web
{
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Mvc;
    using BinaryStudio.TaskManager.Logic.Domain;

    using Ninject;
    using Ninject.Web.Common;

    public class MvcApplication : NinjectHttpApplication
    {
        private readonly DataBaseContext dataBaseContext = new DataBaseContext();
        private  IReminderSender reminderSender;
        private IKernel kernel;
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler());
        }
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Landing",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }

        protected override IKernel CreateKernel()
        {
            kernel = new StandardKernel();
            GlobalHost.DependencyResolver = new NinjectDependencyResolver(kernel);
            RouteTable.Routes.MapHubs();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new DatabaseInitializer());
            reminderSender = kernel.Get<IReminderSender>();
            
            reminderSender.StartTimer();
        }
    }
}