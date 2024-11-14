using Caramel.Pattern.Services.Application.Services.Adopters;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Services.Pattern.Tests.Mocks.Data;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;

namespace Caramel.Services.Pattern.Tests.Application.Adopters
{
    [ExcludeFromCodeCoverage]
    public class AdopterVerificationCodeServiceTest
    {
        [Fact]
        public async Task SendConfirmation_Success()
        {
            var partnerServiceMock = new Mock<IAdoptersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            partnerServiceMock.Setup(x => x.GetAdopterByEmailAsync(It.IsAny<string>())).ReturnsAsync(AdopterData.Data["Basic"]);
            emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();
            unitOfWorkMock.Setup(x => x.AdoptersVerificationCodes.AddAsync(It.IsAny<AdopterVerificationCode>())).Verifiable();

            var service = new AdoptersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            await service.SendConfirmation("test@basic.com");

            emailSenderMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            unitOfWorkMock.Verify(x => x.AdoptersVerificationCodes.AddAsync(It.IsAny<AdopterVerificationCode>()), Times.Once);

        }

        [Fact]
        public async Task VerificationCodeValidate_Success()
        {
            var partnerServiceMock = new Mock<IAdoptersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.AdoptersVerificationCodes.GetSingleAsync(It.IsAny<Expression<Func<AdopterVerificationCode, bool>>>()))
                .ReturnsAsync(AdopterVerificationCodeData.Data["Basic"]);

            var service = new AdoptersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var isValid = await service.VerificationCodeValidate("t35t3", "123456");

            Assert.True(isValid);
        }

        [Fact]
        public async Task VerificationCodeValidate_WithoutEmail()
        {
            var partnerServiceMock = new Mock<IAdoptersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.AdoptersVerificationCodes.GetSingleAsync(It.IsAny<Expression<Func<AdopterVerificationCode, bool>>>()))
                .ReturnsAsync(AdopterVerificationCodeData.Data["Basic"]);

            var service = new AdoptersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await service.VerificationCodeValidate("", "123456"));

            Assert.Contains("O Email é obrigatório.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task VerificationCodeValidate_BadRequest()
        {
            var partnerServiceMock = new Mock<IAdoptersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.AdoptersVerificationCodes.GetSingleAsync(It.IsAny<Expression<Func<AdopterVerificationCode, bool>>>()))
                .ReturnsAsync(AdopterVerificationCodeData.Data["Invalid"]);

            var service = new AdoptersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await service.VerificationCodeValidate("t35t3", "123456"));

            Assert.Contains("Código de verificação Expirado.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.Failure, exception.Status);
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task VerificationCodeValidate_InvalidCode()
        {
            var partnerServiceMock = new Mock<IAdoptersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.AdoptersVerificationCodes.GetSingleAsync(It.IsAny<Expression<Func<AdopterVerificationCode, bool>>>()))
                .ReturnsAsync(AdopterVerificationCodeData.Data["Basic"]);

            var service = new AdoptersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var isValid = await service.VerificationCodeValidate("t35t3", "54321");

            Assert.False(isValid);
        }
    }
}
