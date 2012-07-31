// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfileControllerTests.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProfileControllerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Tests
{
    using BinaryStudio.TaskManager.Logic.Core;
    using BinaryStudio.TaskManager.Web.Controllers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ProfileControllerTests
    {
        private Mock<IUserRepository> userRepositoryMock;

        /// <summary>
        /// The user processor mock.
        /// </summary>
        private Mock<IUserProcessor> userProcessorMock;

        /// <summary>
        /// The project processor mock.
        /// </summary>
        private Mock<IProjectProcessor> projectProcessorMock;

        /// <summary>
        /// The controller.
        /// </summary>
        private ProfileController controller;

        /// <summary>
        /// The init.
        /// </summary>
        public void Init()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userProcessorMock = new Mock<IUserProcessor>();
            this.projectProcessorMock = new Mock<IProjectProcessor>();

            this.controller = new ProfileController(
                this.userProcessorMock.Object, this.projectProcessorMock.Object, this.userRepositoryMock.Object);
        }
    }
}
