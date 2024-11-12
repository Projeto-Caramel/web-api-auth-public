using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.Auth;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Services.Partners;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
using System.Text.Json;

namespace Caramel.Pattern.Services.Api.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    public class PartnerController : BaseController
    {
        private readonly ILogger<PartnerController> _logger;
        private readonly IPartnerService _service;
        private readonly ITokenService _tokenService;
        private readonly ICipherService _cipherService;
        private readonly IPartnerVerificationCodeService _partnerVerificationCodeService;
        private readonly IMapper _mapper;
        private readonly IPartnersApiService _partnerApiService;

        public PartnerController(
            ILogger<PartnerController> logger,
            IPartnerService service,
            ITokenService tokenService,
            ICipherService cipherService,
            IPartnerVerificationCodeService partnerVerificationCodeService,
            IMapper mapper,
            IPartnersApiService partnerApiService)
        {
            _logger = logger;
            _service = service;
            _tokenService = tokenService;
            _cipherService = cipherService;
            _partnerVerificationCodeService = partnerVerificationCodeService;
            _mapper = mapper;
            _partnerApiService = partnerApiService;
        }

        /// <summary>
        /// Verifica login do usuário
        /// </summary>
        /// <param name="request">Request de Email e Senha</param>
        /// <returns>Custom Response de Authenticação</returns>
        [HttpPost("/auth/partner/authenticate")]
        [ProducesResponseType(typeof(CustomAuthResponse<Partner>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AuthAsync(AdopterAuthenticateRequest request)
        {
            request.Password = _cipherService.Encrypt(request.Password);

            var result = await _service.ValidateLoginAsync(request.Email, request.Password);

            var token = _tokenService.GenerateJwtTokenAsync(result.Id, result.Name, result.Role, result.ProfileImageUrl);

            var response = new CustomAuthResponse<Partner>(StatusProcess.Success, token);

            _logger.LogInformation(new EventId(), "200 - OK", JsonSerializer.Serialize(response));

            return Ok(response);
        }

        /// <summary>
        /// Envia código de Autenticação para o email do Parceiro cadastrado
        /// </summary>
        /// <param name="email">Email do Parceiro cadastrado</param>
        /// <returns>Custom Email Response</returns>
        [HttpPost("/auth/partner/verification/email")]
        [ProducesResponseType(typeof(CustomEmailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendVerificationCodeEmail(string email)
        {
            var partner = await _partnerVerificationCodeService.SendConfirmation(email);

            return Ok(new CustomEmailResponse(partner.Id, StatusProcess.Success));
        }

        /// <summary>
        /// Valida o código inserido pelo usuário
        /// </summary>
        /// <param name="userId">ID único do usuário</param>
        /// <param name="code">Código inserido</param>
        /// <returns>Custom Verification Code Response de uma Token Model</returns>
        [HttpPost("/auth/partner/verification/validate")]
        [ProducesResponseType(typeof(CustomVerificationCodeResponse<TokenModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerificationCodeValidate(string userId, string code)
        {
            var isValid = await _partnerVerificationCodeService.VerificationCodeValidate(userId, code);

            var partner = await _partnerApiService.GetSingleOrDefaultByIdAsync(userId);

            var token = _tokenService.GenerateJwtTokenAsync(partner.Id, partner.Name, partner.Role, partner.ProfileImageUrl);

            return isValid ?
                Ok(new CustomVerificationCodeResponse<TokenModel>(token, StatusProcess.Success)) :
                throw new BusinessException("Código inválido!", StatusProcess.InvalidRequest, HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Registra um usuário com uma senha aleatório no Banco de dados
        /// </summary>
        /// <param name="request">Request com dados básicos do parceiro</param>
        /// <returns>Usuário criado</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("/auth/partner/register")]
        [ProducesResponseType(typeof(CustomVerificationCodeResponse<TokenModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAsync(PartnerRegistrationRequest request)
        {
            var entity = _mapper.Map<PartnerRegistrationRequest, Partner>(request);

            var result = await _service.RegisterAsync(entity);

            var token = _tokenService.GenerateJwtTokenAsync(result.Id, result.Name, result.Role, result.ProfileImageUrl);

            var response = new CustomAuthResponse<Partner>(StatusProcess.Success, token);

            _logger.LogInformation(new EventId(), "201 - Created", JsonSerializer.Serialize(response));

            return Created(string.Empty, response);
        }

        /// <summary>
        /// Atualiza a senha de um usuário no Banco de dados
        /// </summary>
        /// <param name="request">ID e nova senha</param>
        /// <returns>Dados do usuáro atualizados</returns>
        [Authorize]
        [HttpPut("/auth/partner/password/change")]
        [ProducesResponseType(typeof(CustomVerificationCodeResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangePassword(PartnerUpdatePasswordRequest request)
        {
            request.Password = _cipherService.Encrypt(request.Password);

            var partner = await _service.UpdatePasswordAsync(request.Id, request.Password);

            return Ok(new CustomUpdateResponse<Partner>(partner, StatusProcess.Success));
        }
    }
}
