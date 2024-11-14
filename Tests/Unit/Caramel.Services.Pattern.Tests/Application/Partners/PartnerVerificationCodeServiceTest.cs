using Caramel.Pattern.Services.Application.Services.Partners;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
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

namespace Caramel.Services.Pattern.Tests.Application.Partners
{
    [ExcludeFromCodeCoverage]
    public class PartnerVerificationCodeServiceTest
    {
        [Fact]
        public async Task SendConfirmation_Success()
        {
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            partnerServiceMock.Setup(x => x.GetPartnerByEmailAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Basic"]);
            emailSenderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null)).Verifiable();
            unitOfWorkMock.Setup(x => x.PartnerVerificationCodes.AddAsync(It.IsAny<PartnerVerificationCode>())).Verifiable();

            var service = new PartnersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            await service.SendConfirmation("test@basic.com");

            emailSenderMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            unitOfWorkMock.Verify(x => x.PartnerVerificationCodes.AddAsync(It.IsAny<PartnerVerificationCode>()), Times.Once);

        }

        [Fact]
        public async Task SendConfirmation_BusinessException()
        {
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            partnerServiceMock.Setup(x => x.GetSingleOrDefaultByIdAsync(It.IsAny<string>())).ReturnsAsync(PartnerData.Data["Null"]);

            var service = new PartnersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await service.SendConfirmation("t35t3"));

            Assert.Contains("Usuário não existe.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task VerificationCodeValidate_Success()
        {
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.PartnerVerificationCodes.FetchAsync(It.IsAny<Expression<Func<PartnerVerificationCode, bool>>>()))
                .ReturnsAsync([PartnerVerificationCodeData.Data["Basic"]]);

            var service = new PartnersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var isValid = await service.VerificationCodeValidate("t35t3", "123456");

            Assert.True(isValid);
        }

        [Fact]
        public async Task VerificationCodeValidate_WithoutId()
        {
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.PartnerVerificationCodes.FetchAsync(It.IsAny<Expression<Func<PartnerVerificationCode, bool>>>()))
                .ReturnsAsync([PartnerVerificationCodeData.Data["Basic"]]);

            var service = new PartnersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await service.VerificationCodeValidate("", "123456"));

            Assert.Contains("O Id é obrigatório.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.InvalidRequest, exception.Status);
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
        }

        [Fact]
        public async Task VerificationCodeValidate_Timeout()
        {
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.PartnerVerificationCodes.FetchAsync(It.IsAny<Expression<Func<PartnerVerificationCode, bool>>>()))
                .ReturnsAsync([PartnerVerificationCodeData.Data["Invalid"]]);

            var service = new PartnersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await service.VerificationCodeValidate("t35t3", "123456"));

            Assert.Contains("Código de verificação Expirado.", exception.ErrorDetails);
            Assert.Equal(StatusProcess.Failure, exception.Status);
            Assert.Equal(HttpStatusCode.RequestTimeout, exception.StatusCode);
        }

        [Fact]
        public async Task VerificationCodeValidate_InvalidCode()
        {
            var partnerServiceMock = new Mock<IPartnersApiService>();
            var emailSenderMock = new Mock<IEmailSender>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(x => x.PartnerVerificationCodes.FetchAsync(It.IsAny<Expression<Func<PartnerVerificationCode, bool>>>()))
                .ReturnsAsync([PartnerVerificationCodeData.Data["Basic"]]);

            var service = new PartnersVerificationCodeService(unitOfWorkMock.Object, emailSenderMock.Object, partnerServiceMock.Object);

            var isValid = await service.VerificationCodeValidate("t35t3", "54321");

            Assert.False(isValid);
        }
    }
}
