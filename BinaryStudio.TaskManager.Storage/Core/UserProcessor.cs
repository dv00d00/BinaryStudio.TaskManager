// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProcessor.cs" company="">
//   
// </copyright>
// <summary>
//   The user processor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;

    using BinaryStudio.TaskManager.Logic.Domain;

    /// <summary>
    /// The user processor.
    /// </summary>
    public class UserProcessor : IUserProcessor
    {
        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The cryptography provider.
        /// </summary>
        private readonly ICryptoProvider cryptoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProcessor"/> class.
        /// </summary>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        /// <param name="cryptoProvider">
        /// The crypto provider.
        /// </param>
        public UserProcessor(IUserRepository userRepository, ICryptoProvider cryptoProvider)
        {
            this.userRepository = userRepository;
            this.cryptoProvider = cryptoProvider;
        }

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="linkedInId">
        /// The linked in id.
        /// </param>
        /// <param name="imageData">
        /// The image data.
        /// </param>
        /// <param name="imageMimeType">
        /// The image mime type.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool CreateUser(string userName, string password, string email, string linkedInId, byte[] imageData, string imageMimeType)
        {
            var user = this.userRepository.GetByName(userName);
            if (user != null)
            {
                return false;
            }
            
            var salt = this.cryptoProvider.CreateSalt();
            var newUser = new User
            {
                UserName = userName,
                RoleId = 2,
                Credentials = new Credentials
                                  {
                                      Passwordhash = this.cryptoProvider.CreateCryptoPassword(password, salt),
                                      Salt = salt,
                                      IsVerify = true
                                  },
                Email = email,
                LinkedInId = linkedInId,
                ImageData = imageData,
                ImageMimeType = imageMimeType,
                IsDeleted = false
            };

            this.userRepository.CreateUser(newUser);
            return true;
        }

        /// <summary>
        /// The set role to user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        public void SetRoleToUser(string userName, string roleName)
        {
            if (!Roles.RoleExists(roleName))
            {
                Roles.CreateRole(roleName);
            }

            if (!Roles.IsUserInRole(userName, roleName))
            {
                Roles.AddUserToRole(userName, roleName);
            }
        }

        /// <summary>
        /// The set role to user from db.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        public void SetRoleToUserFromDB(string userName)
        {
            if (!Roles.RoleExists(this.userRepository.GetRoleByName(userName)))
            {
                Roles.CreateRole(this.userRepository.GetRoleByName(userName));
            }

            if (!Roles.IsUserInRole(userName, this.userRepository.GetRoleByName(userName)))
            {
                Roles.AddUserToRole(userName, this.userRepository.GetRoleByName(userName));
            }
        }

        /// <summary>
        /// Verification, is user in role "admin"
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool IsAdmin(string userName)
        {
            return Roles.IsUserInRole("admin");
        }

        /// <summary>
        /// The log on user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool LogOnUser(string userName, string password)
        {
            var user = this.userRepository.GetByName(userName);
            if (user == null || user.IsDeleted)
            {
                return false;
            }

            if (this.cryptoProvider.ComparePassword(user.Credentials.Passwordhash, user.Credentials.Salt, password) && user.Credentials.IsVerify)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, true);
                return true;
            }

            return false;
        }

        /// <summary>
        /// The get user by name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        public User GetUserByName(string userName)
        {
            try
            {
                return this.userRepository.GetByName(userName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        public User GetUser(int userId)
        {
            return this.userRepository.GetById(userId);
        }

        /// <summary>
        /// The get user by linked in id.
        /// </summary>
        /// <param name="linkedinId">
        /// The linkedIn id.
        /// </param>
        /// <returns>
        /// The BinaryStudio.TaskManager.Logic.Domain.User.
        /// </returns>
        public User GetUserByLinkedInId(string linkedinId)
        {
            try
            {
                return this.userRepository.GetAll().ToList().Single(it => it.LinkedInId == linkedinId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.userRepository.GetAll();
        }
    }
}
