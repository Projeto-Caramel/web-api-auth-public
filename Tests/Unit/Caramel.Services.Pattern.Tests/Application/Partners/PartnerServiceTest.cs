using AutoMapper;
using Caramel.Pattern.Services.Application.Services.Partners;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners;
using Caramel.Pattern.Services.Domain.Repositories;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Security;
using Caramel.Services.Pattern.Tests.Mocks.Data;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerServiceTest
    {

        [Fact]
        public async Task RegisterAsync_Success()
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var mapperMock = new Mock<IMapper>();

            cipherMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("Teste#123");
            cipherMock.Setup(x => x.GenerateRandomPassword()).Returns("dTy7m726FLaYCQWMdzOYTg==");

            partnerServiceMock.Setup(x => x.RegisterPartnerAsync(It.IsAny<PartnerRegistrationApiRequest>())).ReturnsAsync(PartnerData.Data["Basic"]);

            var service = new PartnerService(cipherMock.Object, emailSenderMock.Object, partnerServiceMock.Object, mapperMock.Object);

            var user = await service.RegisterAsync(PartnerData.Data["Basic"]);

            Assert.NotNull(user);
            Assert.Equivalent(PartnerData.Data["Basic"], user);
        }

        [Fact]
        public async Task RegisterAsync_InvalidEntity()
        {
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var mapperMock = new Mock<IMapper>();

            var service = new PartnerService(cipherMock.Object, emailSenderMock.Object, partnerServiceMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
                 service.RegisterAsync(PartnerData.Data["Empty"]));

            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task ValidateLoginAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var cipherMock = new Mock<ICipherService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var mapperMock = new Mock<IMapper>();

            unitOfWorkMock.Setup(x => x.Partners.GetSingleAsync(It.IsAny<Expression<Func<Partner, bool>>>())).ReturnsAsync(PartnerData.Data["Basic"]);
            unitOfWorkMock.Setup(x => x.Partners.AddAsync(It.IsAny<Partner>())).Verifiable();
            cipherMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("Teste#123");
            partnerServiceMock.Setup(x => x.GetPartnerByEmailAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);

            var service = new PartnerService(cipherMock.Object, emailSenderMock.Object, partnerServiceMock.Object, mapperMock.Object);

            var user = await service.ValidateLoginAsync("test@basic.com", "dTy7m726FLaYCQWMdzOYTg==");

            Assert.NotNull(user);
            Assert.Equivalent(PartnerData.Data["Basic"], user);
        }

        [Theory]
        [InlineData("Basic", "Teste", "", "dTy7m726FLaYCQWMdzOYTg==", "O E-mail é obrigatório.", HttpStatusCode.UnprocessableEntity)]
        [InlineData("Basic", "Teste", "test@basic.com", "", "A Senha é obrigatória.", HttpStatusCode.UnprocessableEntity)]
        [InlineData("Basic", "Teste", "testbasic.com", "dTy7m726FLaYCQWMdzOYTg==", "E-mail inválido.", HttpStatusCode.UnprocessableEntity)]
        [InlineData("Null", "Teste", "test@basic.com", "dTy7m726FLaYCQWMdzOYTg==", "Parceiro não encontrado na nossa base de dados.", HttpStatusCode.Unauthorized)]

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
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var mapperMock = new Mock<IMapper>();

            cipherMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(passwordKey);

            var service = new PartnerService(cipherMock.Object, emailSenderMock.Object, partnerServiceMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
               service.ValidateLoginAsync(emailRequest, passwordRequest));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(statusCode, exception.StatusCode);
        }

        [Fact]
        public async Task UpdatePasswordAsync_Success()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var cipherMock = new Mock<ICipherService>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var emailSenderMock = new Mock<IEmailSender>();
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var mapperMock = new Mock<IMapper>();

            cipherMock.Setup(x => x.Decrypt("dTy7m726FLaYCQWMdzOYTg==")).Returns("Test#123");
            cipherMock.Setup(x => x.Decrypt("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs")).Returns("Test@123");
            cipherMock.Setup(x => x.ValidatePasswordPolicy("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs")).Returns(true);

            partnerServiceMock.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);
            partnerServiceMock.Setup(x => x.UpdatePartnerPassword(It.IsAny<string>(), It.IsAny<PartnerUpdatePasswordApiRequest>())).ReturnsAsync(PartnerData.Data["Basic"]);

            mapperMock.Setup(x => x.Map<PartnerUpdatePasswordApiRequest>(It.IsAny<Partner>())).Returns(PartnerUpdatePasswordRequestData.Data["Basic"]);

            var service = new PartnerService(cipherMock.Object, emailSenderMock.Object, partnerServiceMock.Object, mapperMock.Object);

            var result = await service.UpdatePasswordAsync("t35t3", "EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs");

            Assert.Equal("EAAAAFrj9qr7OVPsc8RIIyql6l0Pvp1FlwVgPSfiNU5x+pPs", result.Password);
        }

        [Theory]
        [InlineData("", "", "Basic", "O Id é obrigatório.")]
        [InlineData("t35t3", "", "Basic", "A Senha é obrigatória.")]
        [InlineData("t35t3", "dTy7m726FLaYCQWMdzOYTg==", "Null", "Parceiro não encontrado na nossa base de dados.")]
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
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var mapperMock = new Mock<IMapper>();

            cipherMock.Setup(x => x.Decrypt("dTy7m726FLaYCQWMdzOYTg==")).Returns("Test#123");
            cipherMock.Setup(x => x.Decrypt("XW+cxE2WUFQMkjEKgwT2Hw==")).Returns("test");
            cipherMock.Setup(x => x.ValidatePasswordPolicy("XW+cxE2WUFQMkjEKgwT2Hw=="))
                .Throws(new BusinessException("Padrão de senha inválido.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity));

            partnerServiceMock.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data[key]);
            partnerServiceMock.Setup(x => x.UpdatePartnerPassword(It.IsAny<string>(), It.IsAny<PartnerUpdatePasswordApiRequest>())).ReturnsAsync(PartnerData.Data["Basic"]);

            mapperMock.Setup(x => x.Map<PartnerUpdatePasswordApiRequest>(It.IsAny<Partner>())).Returns(PartnerUpdatePasswordRequestData.Data[key]);

            var service = new PartnerService(cipherMock.Object, emailSenderMock.Object, partnerServiceMock.Object, mapperMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(() =>
              service.UpdatePasswordAsync(id, password));

            Assert.Contains(message, exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }
    }
}
