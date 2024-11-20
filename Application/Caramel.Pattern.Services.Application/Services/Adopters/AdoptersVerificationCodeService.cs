using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Application.Services.Helpers;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services.Adopters
{
    public class AdoptersVerificationCodeService : BaseVerificationCodeService, IAdopterVerificationCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IAdoptersApiService _adopterApiService;

        public AdoptersVerificationCodeService(IUnitOfWork unitOfWork, IEmailSender emailSender, IAdoptersApiService adoptersApiService)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _adopterApiService = adoptersApiService;
        }

        public async Task SendConfirmation(string email)
        {
            var code = GenerateCode();

            var verificationCode = new AdopterVerificationCode
            {
                UserEmail = email,
                Code = code,
                ValidTo = DateTime.Now.AddMinutes(10)
            };

            var entity = await _unitOfWork.AdoptersVerificationCodes.GetSingleAsync(x => x.UserEmail == email);

            if (entity is null)
            {
                await _unitOfWork.AdoptersVerificationCodes.AddAsync(verificationCode);

                var entityRegistered = await _unitOfWork.AdoptersVerificationCodes.GetSingleAsync(x => x.UserEmail == email);

                await _emailSender.SendEmailAsync(
                    email,
                    EmailTemplateHelper.GetTemporaryConfirmationCode(code),
                    "Código de Autenticação");

                return;
            }
            else
            {
                entity.Code = code;
                entity.ValidTo = DateTime.Now.AddMinutes(10);

                _unitOfWork.AdoptersVerificationCodes.Update(entity);

                await _emailSender.SendEmailAsync(
                    email,
                    EmailTemplateHelper.GetTemporaryConfirmationCode(code),
                    "Código de Autenticação");

                return;
            }
        }

        public async Task<bool> VerificationCodeValidate(string email, string code)
        {
            if (string.IsNullOrEmpty(email))
                throw new BusinessException("O Email é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var verificationCode = await _unitOfWork.AdoptersVerificationCodes.GetSingleAsync(x => x.UserEmail == email);

            if (verificationCode == null)
                throw new BusinessException("Código do Usuário ou Código de Verificação incorretos.", StatusProcess.Failure, HttpStatusCode.BadRequest);

            if (verificationCode.ValidTo < DateTime.Now)
                throw new BusinessException("Código de verificação Expirado.", StatusProcess.Failure, HttpStatusCode.BadRequest);

            return verificationCode.Code == code ? true : false;
        }
    }
}
