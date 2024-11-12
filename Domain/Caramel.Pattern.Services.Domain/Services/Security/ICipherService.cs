namespace Caramel.Pattern.Services.Domain.Services.Security
{
    public interface ICipherService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        bool ValidatePasswordPolicy(string password);
        bool ValidatePassword(string password, string userPassword);
        string GenerateRandomPassword();
    }
}
