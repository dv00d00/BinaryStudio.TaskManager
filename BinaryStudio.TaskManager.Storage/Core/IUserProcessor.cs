using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System.Collections.Generic;

    public interface IUserProcessor
    {        

        void SetRoleToUser(string userName, string roleName);

        void SetRoleToUserFromDB(string userName);

        bool IsAdmin(string userName);

        bool LogOnUser(string userName, string password);

        User GetUserByName(string userName);

        bool CreateUser(string userName, string password, string email, string linkedInId, byte[] imageData, string imageMimeType);

        User GetUser(int userId);

        User GetUserByLinkedInId(string linkedinId);

        IEnumerable<User> GetAllUsers();

        void AddNews(News news);

        IEnumerable<News> GetAllNewsForUser(int userId);
    }

}
