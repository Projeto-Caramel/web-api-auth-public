using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Caramel.Pattern.Services.Application.Services.Security
{
    public class CipherService : ICipherService
    {
        private readonly string _key;

        public CipherService(IConfiguration configuration)
        {
            _key = configuration["EncryptionKey"];

            if (string.IsNullOrEmpty(_key))
                throw new ArgumentException("Chave de criptografia não encontrada na configuração.");
        }

        public string Encrypt(string plainText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(_key);
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using MemoryStream msDecrypt = new(fullCipher);
            using Aes aesAlg = Aes.Create();
            byte[] iv = new byte[sizeof(int)];
            msDecrypt.Read(iv, 0, iv.Length);
            iv = new byte[BitConverter.ToInt32(iv, 0)];
            msDecrypt.Read(iv, 0, iv.Length);

            aesAlg.Key = Encoding.UTF8.GetBytes(_key);
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }

        public bool ValidatePasswordPolicy(string password)
        {
            var decryptedPassword = Decrypt(password);

            if (decryptedPassword.Length < 8 && !decryptedPassword.Any(char.IsSymbol))
                throw new BusinessException("Padrão de senha inválido.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            return true;
        }

        public bool ValidatePassword(string password, string userPassword)
        {
            var requestPassword = Decrypt(password);
            var userRequest = Decrypt(userPassword);

            if (requestPassword != userRequest)
                throw new BusinessException("Email e/ou Senha Inválidos.", StatusProcess.InvalidRequest, HttpStatusCode.Unauthorized);

            return true;
        }

        public string GenerateRandomPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&*()-_=+[]{}|;:,.<>?";

            var rand = new Random();

            StringBuilder password = new StringBuilder();
            password.Append(symbols[rand.Next(symbols.Length)]);

            string allCharacters = upperCase + lowerCase + numbers + symbols;

            for (int i = 1; i < 8; i++)
            {
                password.Append(allCharacters[rand.Next(allCharacters.Length)]);
            }

            var result = new string(password.ToString().ToCharArray().OrderBy(s => rand.Next()).ToArray());

            return Encrypt(result);
        }

    }
}
