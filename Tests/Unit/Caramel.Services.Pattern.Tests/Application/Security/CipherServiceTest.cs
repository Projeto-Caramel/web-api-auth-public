using Caramel.Pattern.Services.Application.Services.Security;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Security
{
    [ExcludeFromCodeCoverage]
    public class CipherServiceTest
    {
        public readonly CipherService _service;

        public CipherServiceTest()
        {
            var configurationMock = new Mock<IConfiguration>();
            var configurationSectionMock = new Mock<IConfigurationSection>();

            configurationSectionMock.Setup(x => x.Value).Returns("testeKEY6kyfk775ciufy7==");
            configurationMock.Setup(x => x.GetSection(It.IsAny<string>())).Returns(configurationSectionMock.Object);
            configurationMock.Setup(x => x[It.IsAny<string>()]).Returns("testeKEY6kyfk775ciufy7==");

            _service = new CipherService(configurationMock.Object);
        }

        [Fact]
        public void EncryptAndDecrypt_Success()
        {
            var plainText = "teste";
            var cipherText = _service.Encrypt(plainText);
            var result = _service.Decrypt(cipherText);

            Assert.False(string.IsNullOrEmpty(result));
            Assert.Equal(plainText, result);
        }

        [Fact]
        public void GenerateValidRandonPassword_Success()
        {
            var cipherText = _service.GenerateRandomPassword();
            var decryptedPassword = _service.Decrypt(cipherText);

            var isValid = decryptedPassword.Length < 8 && !decryptedPassword.Any(char.IsSymbol) ? false : true;

            Assert.True(isValid);
        }

        [Fact]
        public void ValidatePasswordPolicy_Success()
        {
            var encryptedPassword = _service.Encrypt("Teste#123");
            var isValid = _service.ValidatePasswordPolicy(encryptedPassword);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidatePasswordPolicy_BusinessException()
        {
            var encryptedPassword = _service.Encrypt("Test123");

            var exception = Assert.Throws<BusinessException>(() =>
            {
                var isValid = _service.ValidatePasswordPolicy(encryptedPassword);
            });

            Assert.Contains("Padrão de senha inválido.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public void ValidatePassword_Success()
        {
            var encryptedPassword = "EAAAAIsaTBxl19JwHhLgEqZgpvbkDcxAz5yVuHpwuAGEaM33";

            var isValid = _service.ValidatePassword("EAAAAIsaTBxl19JwHhLgEqZgpvbkDcxAz5yVuHpwuAGEaM33", encryptedPassword);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidatePassword_BusinessException()
        {
            var encryptedPassword = "EAAAAIsaTBxl19JwHhLgEqZgpvbkDcxAz5yVuHpwuAGEaM33";

            var exception = Assert.Throws<BusinessException>(() =>
            {
                var isValid = _service.ValidatePassword("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs", encryptedPassword);
            });

            Assert.Contains("Email e/ou Senha Inválidos.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        }
    }
}
