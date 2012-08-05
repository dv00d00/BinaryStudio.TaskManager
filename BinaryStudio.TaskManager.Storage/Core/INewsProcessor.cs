using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INewsProcessor
    {
    }

    class NewsProcessor : INewsProcessor
    {
        private readonly INewsRepository newsRepository;
        private readonly INotifier notifier;

        public NewsProcessor(INotifier notifier, INewsRepository newsRepository)
        {
            this.notifier = notifier;
            this.newsRepository = newsRepository;
        }
    }
}
