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
    }
}