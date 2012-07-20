// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptoProvider.cs" company="">
//   
// </copyright>
// <summary>
//   The crypto provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Logic.Core
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The crypto provider.
    /// </summary>
    public class CryptoProvider : ICryptoProvider
    {
        /// <summary>
        /// The create salt.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public string CreateSalt()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The create hash.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public string CreateHash(string password)
        {
            var md5 = MD5.Create();
            var encoding = Encoding.UTF8;
            return Convert.ToBase64String(md5.ComputeHash(encoding.GetBytes(password)));
        }

        /// <summary>
        /// The create crypto password.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="salt">
        /// The salt.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public string CreateCryptoPassword(string password, string salt)
        {
            return this.CreateHash(this.CreateHash(password) + salt);
        }

        /// <summary>
        /// The compare password.
        /// </summary>
        /// <param name="passwordHash">
        /// The user password hash.
        /// </param>
        /// <param name="passwordSalt">
        /// The user password salt.
        /// </param>
        /// <param name="password">
        /// The current password.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool ComparePassword(string passwordHash, string passwordSalt, string password)
        {
            return passwordHash == this.CreateCryptoPassword(password, passwordSalt);
        }
    }
}