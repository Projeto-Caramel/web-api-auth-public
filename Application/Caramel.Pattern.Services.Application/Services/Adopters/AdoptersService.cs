using AutoMapper;
using Caramel.Pattern.Services.Application.Services.Base;
using Caramel.Pattern.Services.Application.Services.Helpers;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Adopters;
using Caramel.Pattern.Services.Domain.Repositories.UnitOfWork;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using Caramel.Pattern.Services.Domain.Services.Security;
using Caramel.Pattern.Services.Domain.Validators;
using System.Net;
using System.Text.RegularExpressions;

namespace Caramel.Pattern.Services.Application.Services.Adopters
{
    public class AdoptersService : BaseService, IAdoptersService
    {
        private readonly ICipherService _cipherService;
        private readonly IEmailSender _emailSender;
        private readonly IAdoptersApiService _adoptersApiService;
        private readonly IPartnersApiService _partnerApiService;
        private readonly IMapper _mapper;

        public AdoptersService(
            ICipherService cipherService,
            IEmailSender emailSender,
            IAdoptersApiService adoptersApiService,
            IPartnersApiService partnersApiService,
            IMapper mapper)
        {
            _cipherService = cipherService;
            _emailSender = emailSender;
            _adoptersApiService = adoptersApiService;
            _partnerApiService = partnersApiService;
            _mapper = mapper;
        }

        public async Task<Adopter> RegisterAsync(Adopter entity)
        {
            BusinessException.ThrowIfNull(entity, "Usuário");

            ValidateEntity<AdoptersValidator, Adopter>(entity);

            _cipherService.ValidatePasswordPolicy(entity.Password);

            var request = new AdopterRegistrationApiRequest()
            {
                Name = entity.Name,
                Birthday = entity.Birthday,
                ResidencyType = entity.ResidencyType,
                Lifestyle = entity.Lifestyle,
                PetExperience = entity.PetExperience,
                HasChildren = entity.HasChildren,
                FinancialSituation = entity.FinancialSituation,
                FreeTime = entity.FreeTime,
                Email = entity.Email,
                Password = entity.Password,
            };

            return await _adoptersApiService.RegisterAdopterAsync(request);
        }

        public async Task<Adopter> ValidateLoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new BusinessException("O E-mail é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
            if (string.IsNullOrEmpty(password))
                throw new BusinessException("A Senha é obrigatória.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            ValidateEmailPolicy(email);

            var adopter = await _adoptersApiService.GetAdopterByEmailAsync(email);

            if (adopter == null)
                throw new BusinessException("Usuário não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.Unauthorized);

            _cipherService.ValidatePassword(password, adopter.Password);

            return adopter;
        }

        public async Task<Adopter> UpdatePasswordAsync(string id, string newPassword)
        {
            if (string.IsNullOrEmpty(id))
                throw new BusinessException("O Id é obrigatório.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
            if (string.IsNullOrEmpty(newPassword))
                throw new BusinessException("A Senha é obrigatória.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var partner = await _adoptersApiService.GetSingleOrDefaultByIdAsync(id);

            if (partner == null)
                throw new BusinessException("Usuário não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var requestPassword = _cipherService.Decrypt(newPassword);
            var partnerPassword = _cipherService.Decrypt(partner.Password);

            if (requestPassword == partnerPassword)
                throw new BusinessException("A Senha não pode ser igual a senha anterior.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            _cipherService.ValidatePasswordPolicy(newPassword);

            partner.Password = newPassword;

            var partnerUpdatePasswordRequest = _mapper.Map<AdopterUpdatePasswordApiRequest>(partner);

            var updatedPartner = await _adoptersApiService.UpdateAdopterPassword(id, partnerUpdatePasswordRequest);

            return updatedPartner;
        }

        public async Task SendInterestEmailAsync(AdopterInfos adopterInfos, PetInfos petInfos, string partnerId)
        {
            BusinessException.ThrowIfNull(adopterInfos, "Informações do Usuário");
            BusinessException.ThrowIfNull(petInfos, "Informações do Pet");
            BusinessException.ThrowIfNull(partnerId, "Id do Parceiro");

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(partnerId) ??
                throw new BusinessException("Parceiro não encontrado na nossa base de dados.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);

            var email = EmailTemplateHelper.GetAdoptionConfirmationEmail(partner, adopterInfos, petInfos);

            await _emailSender.SendEmailAsync(partner.Email, email, "Caramel - Novo Interessado!", adopterInfos.Email);
        }

        private void ValidateEmailPolicy(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            var isValid = Regex.IsMatch(email, pattern);

            if (!isValid) throw new BusinessException("E-mail inválido.", StatusProcess.InvalidRequest, HttpStatusCode.UnprocessableEntity);
        }
    }
}
