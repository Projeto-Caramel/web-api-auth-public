using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Application.Services.Helpers;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Partners;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Partners
{
    public class PartnersVerificationCodeService : BaseVerificationCodeService, IPartnerVerificationCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IPartnersApiService _partnerApiService;

        public PartnersVerificationCodeService(IUnitOfWork unitOfWork, IEmailSender emailSender, IPartnersApiService partnerApiService)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _partnerApiService = partnerApiService;
        }

        public async Task<Partner> SendConfirmation(string email)
        {
            var partner = await _partnerApiService.GetPartnerByEmailAsync(email);

            if (partner == null)
                throw new BusinessException("Usuário não existe.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var code = GenerateCode();

            var verificationCode = new PartnerVerificationCode
            {
                PartnerId = partner.Id,
                Code = code,
                ValidTo = DateTime.Now.AddMinutes(10)
            };

            await _unitOfWork.PartnerVerificationCodes.AddAsync(verificationCode);

            await _emailSender.SendEmailAsync(
                partner.Email,
                EmailTemplateHelper.GetTemporaryConfirmationCode(code),
                "Código de Autenticação");

            return partner;
        }

        public async Task<bool> VerificationCodeValidate(string partnerId, string code)
        {
            if (string.IsNullOrEmpty(partnerId))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var verificationCodes = await _unitOfWork.PartnerVerificationCodes.FetchAsync(x => x.PartnerId == partnerId);

            var lastVerificationCode = verificationCodes.OrderByDescending(x => x.Id).FirstOrDefault();

            if (lastVerificationCode == null)
                throw new BusinessException("Código do Usuário ou Código de Verificação incorretos.", StatusProcess.Failure, HttpStatusCode.RequestTimeout);

            if (lastVerificationCode.ValidTo < DateTime.Now)
                throw new BusinessException("Código de verificação Expirado.", StatusProcess.Failure, HttpStatusCode.RequestTimeout);

            return lastVerificationCode.Code == code ? true : false;
        }
    }
}
