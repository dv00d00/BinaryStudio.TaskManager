// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseInitializer.cs" company="">
//   
// </copyright>
// <summary>
//   The database initializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;

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

            var user = new User
                           {
                               Id = 1, 
                               UserName = "Test User", 
                               Credentials = new Credentials{IsVerify = true},
                               RoleId = 2
                           };
            context.Users.Add(user);            
            context.Projects.Add(new Project
                                     {
                                         Id = 1,
                                         Name = "Test Project",
                                         ProjectUsers = new Collection<User>
                                                            {
                                                                user
                                                            },
                                         Creator = user,
                                         Created = DateTime.Now,
                                         CreatorId = user.Id
                                     });
           
            context.SaveChanges();
        }
    }
}