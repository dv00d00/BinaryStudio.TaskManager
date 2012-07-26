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

    using System;

    using FizzWare.NBuilder.Dates;

    /// <summary>
    /// The database initializer.
    /// </summary>
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        /// <summary>
        /// The seed is initial database function.
        /// </summary>
        /// <param name="context">The current database context.</param>
        protected override void Seed(DataBaseContext context)
        {
            context.Roles.Add(new UserRoles { Id = 1, RoleName = "admin" });
            context.Roles.Add(new UserRoles { Id = 2, RoleName = "simpleEmployee" });            

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
            context.SaveChanges();
        }
    }
}