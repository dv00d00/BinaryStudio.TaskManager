// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseInitializer.cs" company="">
//   
// </copyright>
// <summary>
//   The database initializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System.Data.Entity;

    using BinaryStudio.TaskManager.Logic.Core;

    /// <summary>
    /// The database initializer.
    /// </summary>
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        /// <summary>
        /// The seed is initial database function.
        /// </summary>
        /// <param name="context">
        /// The current database context.
        /// </param>
        protected override void Seed(DataBaseContext context)
        {
<<<<<<< HEAD
            context.Roles.Add(new UserRoles { Id = 1, RoleName = "admin" });
            context.Roles.Add(new UserRoles { Id = 2, RoleName = "simpleEmployee" });

=======
            var projectRepository = new ProjectRepository(context);
            
            //createProjects(projectRepository);

            context.Priorities.Add(new Priority
                                       {
                                           Id = Guid.NewGuid(),
                                           Description = "Low",
                                           Value = 0
                                       });
            context.Priorities.Add(new Priority
            {
                                            Id = Guid.NewGuid(),
                                            Description = "Middle",
                                            Value = 1
            });
            context.Priorities.Add(new Priority
            {
                                            Id = Guid.NewGuid(),
                                            Description = "High",
                                            Value = 2
            });
>>>>>>> 715b24ea12f2d4451d1f27b55174f928386b2726
            context.SaveChanges();
        }
    }
}