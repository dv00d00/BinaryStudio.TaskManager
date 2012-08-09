using System;
using System.Collections.Generic;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface INewsRepository
    {
        void AddNews(News news);

        IEnumerable<News> GetAllNewsForUser(int userId);

        News GetNewsById(int newsId);

        void MarkAsRead(int newsId);

        int GetNewsCount(int userId);

        int GetUnreadNewsCountForUserByName(string userName);

        IEnumerable<News> GetAllUnreadNewsForUser(int userId);

        void MarkAllUnreadNewsForUser(int userId);
    }
}
