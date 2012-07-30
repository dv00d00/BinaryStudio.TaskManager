using System.Collections.ObjectModel;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System;
    using System.Data.Entity;

    using BinaryStudio.TaskManager.Logic.Core;

    /// <summary>
    /// The database initializer.
    /// </summary>
    public class DatabaseInitializer : DropCreateDatabaseAlways<DataBaseContext>
    {
        /// <summary>
        /// The seed is initial database function.
        /// </summary>
        /// <param name="context">The current database context.</param>
        protected override void Seed(DataBaseContext context)
        {
            context.Roles.Add(new UserRoles { Id = 1, RoleName = "admin" });
            context.Roles.Add(new UserRoles { Id = 2, RoleName = "simpleUser" });            

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



            var cryptoProvider = new CryptoProvider();
            var salt = cryptoProvider.CreateSalt();
            var admin = new User
            {
                UserName = "admin",
                RoleId = 1,
                Credentials = new Credentials
                {
                    Passwordhash = cryptoProvider.CreateCryptoPassword("password", salt),
                    Salt = salt,
                    IsVerify = true
                },
                Email = "admin@admin.com",
                LinkedInId = string.Empty,                                
            };
            context.Users.Add(admin);  

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