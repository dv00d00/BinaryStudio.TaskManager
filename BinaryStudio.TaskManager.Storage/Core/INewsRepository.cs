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

        int GetCountOfUnreadedNewses(int userId);

        void MarkAsRead(int newsId);

        int GetNewsCount(int userId);

        int GetUnreadNewsCountForUserByName(string userName);
    }
}
