using System;
using System.Collections.Generic;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;

namespace BinaryStudio.TaskManager.Logic.Tests
{
    [TestFixture]
    public class NewsProcessorTests
    {
        private NewsProcessor newsProcessor;
        private Mock<INewsRepository> newsRepositoryMock;
        private Mock<INotifier> notifierMock;
        private Mock<IProjectProcessor> projectProcessorMock;

        [SetUp]
        public void Setup()
        {
            this.newsRepositoryMock = new Mock<INewsRepository>();
            this.notifierMock = new Mock<INotifier>();
            this.projectProcessorMock = new Mock<IProjectProcessor>();

            this.newsProcessor = new NewsProcessor(
                notifierMock.Object,
                newsRepositoryMock.
                Object, projectProcessorMock.Object
                );
        }

        [Test]
        public void Should_BroadcastNewsViaNotifier_WhenAddNews()
        {
            // arrange
            News news = new News
                            {
                                Id = 1,
                                UserId = 1
                            };

            // act
            this.newsProcessor.AddNews(news);

            // assert
            this.notifierMock.Verify(mock => mock.BroadcastNews(news), Times.AtLeastOnce());
        }

        [Test]
        public void Should_AddNews_WithNewsAsParametr()
        {
            // arrange
            News news = new News
            {
                Id = 1,
                UserId = 1
            };

            // act
            this.newsProcessor.AddNews(news);

            // assert
            this.newsRepositoryMock.Verify(mock => mock.AddNews(news), Times.AtLeastOnce());
        }

        [Test]
        public void Should_AddCorrectNews_WithHumanTaskHistoryAsParametr()
        {
            // arrange 
            User user = new User
                            {
                                UserName = "UserName",
                                Id = 2
                            };
            HumanTaskHistory taskHistory = new HumanTaskHistory
                                               {
                                                   Id = 1,
                                                   Action = "Create",
                                               };
            News news = new News
                            {
                                HumanTaskHistory = taskHistory,
                                HumanTaskHistoryId = taskHistory.Id,
                                Id = 1,
                                IsRead = false,
                                User = user,
                                UserId = user.Id
                            };
            // act

            this.newsProcessor.AddNews(taskHistory, user);

            // assert
            this.newsRepositoryMock.Verify(mock => mock.AddNews(It.Is<News>(x => 
                x.HumanTaskHistoryId == taskHistory.Id
                && x.UserId == user.Id
                && x.User == user
                && x.HumanTaskHistory == taskHistory)), 
                Times.Once());
        }

        [Test]
        public void Should_CreateNewsForUsers()
        {
            // arrange
            User creator = new User{UserName = "Creator", Id = 1};
            User collaborator = new User { UserName = "Collaborator", Id = 2 };
            this.projectProcessorMock.Setup(it => it.GetUsersAndCreatorInProject(1)).Returns(value: new List<User> {creator, collaborator});
            HumanTaskHistory taskHistory = new HumanTaskHistory
                                               {
                                                   Id = 1,
                                                   Action = "Create"
                                               };
            // act
            this.newsProcessor.CreateNewsForUsersInProject(taskHistory,1);
            // assert
            this.newsRepositoryMock.Verify(it => it.AddNews(It.IsAny<News>()), Times.Exactly(2));
        }

    }
}
