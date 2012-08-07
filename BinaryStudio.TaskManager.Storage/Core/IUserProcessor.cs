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

        void UpdateUsersPhoto(int userId, byte[] imageData, string imageMimeType);

        bool ChangePassword(int userId, string oldPassword, string newPassword);

        User GetUser(int userId);

        int? GetUserByTaskId(int taskId);

        User GetUserByLinkedInId(string linkedinId);

        IEnumerable<User> GetAllUsers();
    }

}
