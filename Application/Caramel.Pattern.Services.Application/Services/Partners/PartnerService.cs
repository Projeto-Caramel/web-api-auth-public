using AutoMapper;
using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Application.Services.Helpers;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Partners;
using Caramel.Pattern.Services.Domain.Services.Security;
using Caramel.Pattern.Services.Domain.Validators;
using System.Net;
using System.Text.RegularExpressions;

namespace Caramel.Pattern.Services.Application.Services.Partners
{
    public class PartnerService : BaseService, IPartnerService
    {
        private readonly ICipherService _cipherService;
        private readonly IEmailSender _emailSender;
        private readonly IPartnersApiService _partnerApiService;
        private readonly IMapper _mapper;

        public PartnerService(
            ICipherService cipherService,
            IEmailSender emailSender,
            IPartnersApiService partnerApiService,
            IMapper mapper)
        {
            _cipherService = cipherService;
            _emailSender = emailSender;
            _partnerApiService = partnerApiService;
            _mapper = mapper;
        }

        public async Task<Partner> RegisterAsync(Partner entity)
        {
            BusinessException.ThrowIfNull(entity, "Parceiro");

            ValidateEntity<PartnerValidator, Partner>(entity);

            entity.Password = _cipherService.GenerateRandomPassword();

            _cipherService.ValidatePasswordPolicy(entity.Password);

            var request = new PartnerRegistrationApiRequest()
            {
                Email = entity.Email,
                Cellphone = entity.Cellphone,
                Password = entity.Password,
                Name = entity.Name,
                Type = entity.Type,
                Role = entity.Role,
                MaxCapacity = entity.MaxCapacity
            };

            var partner = await _partnerApiService.RegisterPartnerAsync(request);

            await _emailSender.SendEmailAsync(
                entity.Email,
                EmailTemplateHelper.GetTemporaryPassword(_cipherService.Decrypt(partner.Password)),
                "Primeiro Acesso");

            return partner;
        }

        public async Task<Partner> ValidateLoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new BusinessException("O E-mail é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
            if (string.IsNullOrEmpty(password))
                throw new BusinessException("A Senha é obrigatória.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            ValidateEmailPolicy(email);

            var partner = await _partnerApiService.GetPartnerByEmailAsync(email);

            if (partner == null)
                throw new BusinessException("Parceiro não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.Unauthorized);

            _cipherService.ValidatePassword(password, partner.Password);

            return partner;
        }

        public async Task<Partner> UpdatePasswordAsync(string id, string newPassword)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
            if (string.IsNullOrEmpty(newPassword))
                throw new BusinessException("A Senha é obrigatória.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(id);

            if (partner == null)
                throw new BusinessException("Parceiro não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var requestPassword = _cipherService.Decrypt(newPassword);
            var partnerPassword = _cipherService.Decrypt(partner.Password);

            if (requestPassword == partnerPassword)
                throw new BusinessException("A Senha não pode ser igual a senha anterior.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            _cipherService.ValidatePasswordPolicy(newPassword);

            partner.Password = newPassword;

            var partnerUpdatePasswordRequest = _mapper.Map<PartnerUpdatePasswordApiRequest>(partner);

            var updatedPartner = await _partnerApiService.UpdatePartnerPassword(id, partnerUpdatePasswordRequest);

            return updatedPartner;
        }

        private void ValidateEmailPolicy(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            var isValid = Regex.IsMatch(email, pattern);

            if (!isValid) throw new BusinessException("E-mail inválido.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
        }
    }
}
