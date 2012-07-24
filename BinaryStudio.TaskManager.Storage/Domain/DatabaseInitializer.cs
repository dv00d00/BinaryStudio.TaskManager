// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseInitializer.cs" company="">
//   
// </copyright>
// <summary>
//   The database initializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using FizzWare.NBuilder.Dates;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    using System.Data.Entity;
    using FizzWare.NBuilder;
    using FizzWare.NBuilder.Generators;

    using BinaryStudio.TaskManager.Logic.Core;

    /// <summary>
    /// The database initializer.
    /// </summary>
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        /// <summary>
        /// The crypto provider.
        /// </summary>
        private ICryptoProvider cryptoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInitializer"/> class.
        /// </summary>
        public DatabaseInitializer()
        {
            this.cryptoProvider = new CryptoProvider();
        }

        /// <summary>
        /// The seed is initial database function.
        /// </summary>
        /// <param name="context">
        /// The current database context.
        /// </param>
        protected override void Seed(DataBaseContext context)
        {
            var projectRepository = new ProjectRepository(context);
            
            //createProjects(projectRepository);
            
            context.Roles.Add(new UserRoles { Id = 1, RoleName = "admin" });
            context.Roles.Add(new UserRoles { Id = 2, RoleName = "simpleEmployee" });

            //context.Projects.Add(new Project
            //{
            //    Id = 1,
            //    Created = Convert.ToDateTime("11.11.2011"),
            //    Name = "111",
            //    Description = "asdasdj",
            //    Creator =
            //        {
            //            Id = 1,
            //            UserName = "admin",
            //            Email = "admin@mail.ru",
            //            Credentials =
            //                {
            //                    Salt = "111",
            //                    Passwordhash =
            //                        this.cryptoProvider.CreateCryptoPassword("password", "111"),
            //                    IsVerify = true
            //                },
            //            RoleId = 1
            //        }
            //});

            //context.Projects.Add(new Project
            //{
            //    Id = 2,
            //    Created = Convert.ToDateTime("12.12.2012"),
            //    Name = "222",
            //    Description = "asdasdj",
            //    Creator =
            //    {
            //        Id = 1,
            //        UserName = "sam",
            //        Email = "sam@mail.ru",
            //        Credentials =
            //        {
            //            Salt = "111",
            //            Passwordhash =
            //                this.cryptoProvider.CreateCryptoPassword("password", "111"),
            //            IsVerify = true
            //        },
            //        RoleId = 1
            //    }
            //});
            //context.ProjectsAndUserses.Add(new ProjectsAndUsers {Id = 1, Project = }

            //var salt = this.cryptoProvider.CreateSalt();
            //context.Users.Add(new User
            //{
            //    Id = 1,
            //    UserName = "admin",
            //    Email = "admin@mail.ru",
            //    Credentials =
            //    {
            //        Salt = salt,
            //        Passwordhash = this.cryptoProvider.CreateCryptoPassword("password", salt),
            //        IsVerify = true
            //    },
            //    RoleId = 1
            //});
            //context.Users.Add(new User
            //{
            //    Id = 2,
            //    UserName = "simple",
            //    Email = "simple@mail.ru",
            //    Credentials =
            //    {
            //        Salt = salt,
            //        Passwordhash = this.cryptoProvider.CreateCryptoPassword("password", salt),
            //        IsVerify = true
            //    },
            //    RoleId = 2
            //});

            context.SaveChanges();
        }

        //private void createProjects(ProjectRepository projectRepository)
        //{
        //    var projects = Builder<Project>.CreateListOfSize(10)
        //        .Random(2)
        //        .With(x => x.Created = GetRandom.DateTime(January.The1st, DateTime.Now))
        //        .With(x => x.Name = GetRandom.Phrase(5))
        //        .With(x => x.Description = GetRandom.Phrase(5))
        //        .With(x => x.Id = 0)
        //        .With(x => x.Creator = 1)
        //    .Build();

        //    foreach (var project in projects)
        //    {
        //        projectRepository.Update(project);
        //    }
        //}
    }
}