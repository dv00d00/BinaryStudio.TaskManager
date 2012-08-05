using System;
using BinaryStudio.TaskManager.Logic.Core;
using BinaryStudio.TaskManager.Logic.Domain;
using Moq;
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
        public void Should_AddNews()
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
    }
}
