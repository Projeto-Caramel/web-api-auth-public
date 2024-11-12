using AutoMapper;
using Caramel.Pattern.Services.Application.Services.Adopters;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Adopters;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Security;
using Caramel.Services.Pattern.Tests.Mocks.Data;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Adopterss
{
    [ExcludeFromCodeCoverage]
    public class AdopterServiceTest
    {
        [Fact]
        public async Task RegisterAsync_Success()
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var adopterApiServiceMock = new Mock<IAdoptersApiService>();
            var mapperMock = new Mock<IMapper>();
            var partnerApiServiceMock = new Mock<IPartnersApiService>();

            cipherMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("Teste#123");
            cipherMock.Setup(x => x.GenerateRandomPassword()).Returns("dTy7m726FLaYCQWMdzOYTg==");

            adopterApiServiceMock.Setup(x => x.RegisterAdopterAsync(It.IsAny<AdopterRegistrationApiRequest>())).ReturnsAsync(AdopterData.Data["Basic"]);

            var service = new AdoptersService(cipherMock.Object, emailSenderMock.Object, adopterApiServiceMock.Object, partnerApiServiceMock.Object, mapperMock.Object);

            var user = await service.RegisterAsync(AdopterData.Data["Basic"]);

            Assert.NotNull(user);
            Assert.Equivalent(AdopterData.Data["Basic"], user);
        }

        [Fact]
        public async Task RegisterAsync_InvalidEntity()
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var adopterApiServiceMock = new Mock<IAdoptersApiService>();
            var mapperMock = new Mock<IMapper>();
            var partnerApiServiceMock = new Mock<IPartnersApiService>();

            var service = new AdoptersService(cipherMock.Object, emailSenderMock.Object, adopterApiServiceMock.Object, partnerApiServiceMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                 service.RegisterAsync(AdopterData.Data["Empty"]));

            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ValidateLoginAsync_Success()
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var adopterApiServiceMock = new Mock<IAdoptersApiService>();
            var mapperMock = new Mock<IMapper>();
            var partnerApiServiceMock = new Mock<IPartnersApiService>();

            cipherMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("Teste#123");
            adopterApiServiceMock.Setup(x => x.GetAdopterByEmailAsync(It.IsAny<string>())).ReturnsAsync(AdopterData.Data["Basic"]);

            var service = new AdoptersService(cipherMock.Object, emailSenderMock.Object, adopterApiServiceMock.Object, partnerApiServiceMock.Object, mapperMock.Object);

            var user = await service.ValidateLoginAsync("test@basic.com", "dTy7m726FLaYCQWMdzOYTg==");

            Assert.NotNull(user);
            Assert.Equivalent(AdopterData.Data["Basic"], user);
        }

        [Theory]
        [InlineData("Basic", "Teste", "", "dTy7m726FLaYCQWMdzOYTg==", "O E-mail é obrigatório.", HttpStatusCode.UnprocessableEntity)]
        [InlineData("Basic", "Teste", "test@basic.com", "", "A Senha é obrigatória.", HttpStatusCode.UnprocessableEntity)]
        [InlineData("Basic", "Teste", "testbasic.com", "dTy7m726FLaYCQWMdzOYTg==", "E-mail inválido.", HttpStatusCode.UnprocessableEntity)]
        [InlineData("Null", "Teste", "test@basic.com", "dTy7m726FLaYCQWMdzOYTg==", "Usuário não encontrado na nossa base de dados.", HttpStatusCode.Unauthorized)]

        public async Task ValidateLoginAsync_BusinessExceptions(
            string userDataKey,
            string passwordKey,
            string emailRequest,
            string passwordRequest,
            string message,
            HttpStatusCode statusCode)
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var adopterApiServiceMock = new Mock<IAdoptersApiService>();
            var mapperMock = new Mock<IMapper>();
            var partnerApiServiceMock = new Mock<IPartnersApiService>();

            cipherMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(passwordKey);

            var service = new AdoptersService(cipherMock.Object, emailSenderMock.Object, adopterApiServiceMock.Object, partnerApiServiceMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
               service.ValidateLoginAsync(emailRequest, passwordRequest));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(statusCode, exception.StatusCode);
        }

        [Fact]
        public async Task UpdatePasswordAsync_Success()
        {
            var unitOfWorkMock = new Mock<IPartnersApiService>();
            var cipherMock = new Mock<ICipherService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var emailSenderMock = new Mock<IEmailSender>();
            var adopterApiServiceMock = new Mock<IAdoptersApiService>();
            var mapperMock = new Mock<IMapper>();
            var partnerApiServiceMock = new Mock<IPartnersApiService>();

            cipherMock.Setup(x => x.Decrypt("dTy7m726FLaYCQWMdzOYTg==")).Returns("Test#123");
            cipherMock.Setup(x => x.Decrypt("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs")).Returns("Test@123");
            cipherMock.Setup(x => x.ValidatePasswordPolicy("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs")).Returns(true);

            adopterApiServiceMock.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(AdopterData.Data["Basic"]);
            adopterApiServiceMock.Setup(x => x.UpdateAdopterPassword(It.IsAny<string>(), It.IsAny<AdopterUpdatePasswordApiRequest>())).ReturnsAsync(AdopterData.Data["Basic"]);

            mapperMock.Setup(x => x.Map<AdopterUpdatePasswordApiRequest>(It.IsAny<Adopter>())).Returns(AdopterUpdatePasswordRequestData.Data["Basic"]);

            var service = new AdoptersService(cipherMock.Object, emailSenderMock.Object, adopterApiServiceMock.Object, partnerApiServiceMock.Object, mapperMock.Object);

            var result = await service.UpdatePasswordAsync("t35t3", "EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs");

            Assert.Equal("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs", result.Password);
        }

        [Theory]
        [InlineData("", "", "Basic", "O Id é obrigatório.")]
        [InlineData("t35t3", "", "Basic", "A Senha é obrigatória.")]
        [InlineData("t35t3", "dTy7m726FLaYCQWMdzOYTg==", "Null", "Usuário não encontrado na nossa base de dados.")]
        [InlineData("t35t3", "dTy7m726FLaYCQWMdzOYTg==", "Basic", "A Senha não pode ser igual a senha anterior.")]
        [InlineData("t35t3", "XW+cxE2WUFQMkjEKgwT2Hw==", "Basic", "Padrão de senha inválido.")]
        public async Task UpdatePasswordAsync_BusinessExceptions(
            string id,
            string password,
            string key,
            string message)
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var adopterApiServiceMock = new Mock<IAdoptersApiService>();
            var mapperMock = new Mock<IMapper>();
            var partnerApiServiceMock = new Mock<IPartnersApiService>();

            cipherMock.Setup(x => x.Decrypt("dTy7m726FLaYCQWMdzOYTg==")).Returns("Test#123");
            cipherMock.Setup(x => x.Decrypt("XW+cxE2WUFQMkjEKgwT2Hw==")).Returns("test");
            cipherMock.Setup(x => x.ValidatePasswordPolicy("XW+cxE2WUFQMkjEKgwT2Hw=="))
                .Throws(new BusinessException("Padrão de senha inválido.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity));

            adopterApiServiceMock.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(AdopterData.Data[key]);
            adopterApiServiceMock.Setup(x => x.UpdateAdopterPassword(It.IsAny<string>(), It.IsAny<AdopterUpdatePasswordApiRequest>())).ReturnsAsync(AdopterData.Data["Basic"]);

            mapperMock.Setup(x => x.Map<AdopterUpdatePasswordApiRequest>(It.IsAny<Adopter>())).Returns(AdopterUpdatePasswordRequestData.Data[key]);

            var service = new AdoptersService(cipherMock.Object, emailSenderMock.Object, adopterApiServiceMock.Object, partnerApiServiceMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
              service.UpdatePasswordAsync(id, password));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }
    }
}
