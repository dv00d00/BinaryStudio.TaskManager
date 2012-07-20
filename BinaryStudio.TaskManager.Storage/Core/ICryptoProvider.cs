namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface ICryptoProvider
    {
        string CreateSalt();
        string CreateHash(string password);
        string CreateCryptoPassword(string password, string salt);
        bool ComparePassword(string passwordhash, string passwordSalt, string password);
    }
}