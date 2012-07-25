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