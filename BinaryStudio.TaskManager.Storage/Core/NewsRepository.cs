using System.Collections.Generic;
using System.Data;
using System.Linq;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public class NewsRepository : INewsRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public NewsRepository(DataBaseContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }


        public void AddNews(News news)
        {
            this.dataBaseContext.Entry(news).State = EntityState.Added;
            this.dataBaseContext.SaveChanges();
        }

        public IEnumerable<News> GetAllNewsForUser(int userId)
        {
            return this.dataBaseContext.News.Where(x => x.UserId == userId);
        }

        public News GetNewsById(int newsId)
        {
            return this.dataBaseContext.News.ToList().SingleOrDefault(x => x.Id == newsId);
        }

        public void MarkAsRead(int newsId)
        {
            var news = this.dataBaseContext.News.First(x => x.Id == newsId);
            news.IsRead = true;
            this.dataBaseContext.Entry(news).State = EntityState.Modified;
            this.dataBaseContext.SaveChanges();
        }

        public int GetNewsCount(int userId)
        {
            List<News> news = this.dataBaseContext.News.Where(x => x.UserId == userId && x.IsRead == false).ToList();
            int count = news.Count;
            return count;
        }

        public int GetUnreadNewsCountForUserByName(string userName)
        {
            return this.dataBaseContext.News.Count(x => x.User.UserName == userName && x.IsRead == false);
        }

        public IEnumerable<News> GetAllUnreadNewsForUser(int userId)
        {
            return this.dataBaseContext.News.Where(x => x.UserId == userId && x.IsRead == false);
        }

        public void MarkAllUnreadNewsForUser(int userId)
        {
            var news = this.dataBaseContext.News.Where(x => x.UserId == userId && x.IsRead == false);
            var a = news.Count();
            foreach (var currentNews in news)
            {
                currentNews.IsRead = true;
                this.dataBaseContext.Entry(currentNews).State = EntityState.Modified;
            }
            this.dataBaseContext.SaveChanges();
        }
    }
}