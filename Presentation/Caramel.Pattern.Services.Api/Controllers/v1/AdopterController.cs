using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.Auth;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Entities.Models.Users;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class AdopterController : BaseController
    {
        private readonly ILogger<AdopterController> _logger;
        private readonly IAdoptersService _service;
        private readonly ITokenService _tokenService;
        private readonly ICipherService _cipherService;
        private readonly IAdopterVerificationCodeService _adoptersVerificationCodeService;
        private readonly IMapper _mapper;
        private readonly IAdoptersApiService _adoptersApiService;

        public AdopterController(
            ILogger<AdopterController> logger,
            IAdoptersService service,
            ITokenService tokenService,
            ICipherService cipherService,
            IAdopterVerificationCodeService adopterVerificationCodeService,
            IMapper mapper,
            IAdoptersApiService adopterApiService)
        {
            _logger = logger;
            _service = service;
            _tokenService = tokenService;
            _cipherService = cipherService;
            _adoptersVerificationCodeService = adopterVerificationCodeService;
            _mapper = mapper;
            _adoptersApiService = adopterApiService;
        }

        /// <summary>
        /// Verifica login do usuário
        /// </summary>
        /// <param name="request">Request de Email e Senha</param>
        /// <returns>Custom Response de Authenticação</returns>
        [HttpPost("/auth/adopter/authenticate")]
        [ProducesResponseType(typeof(CustomAuthResponse<Partner>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AuthAsync(AdopterAuthenticateRequest request)
        {
            request.Password = _cipherService.Encrypt(request.Password);

            var result = await _service.ValidateLoginAsync(request.Email, request.Password);

            var token = _tokenService.GenerateJwtTokenAsync(result.Id, result.Name, Roles.User, result.ProfileImageUrl);

            var response = new CustomAuthResponse<Partner>(StatusProcess.Success, token);

            _logger.LogInformation(new EventId(), "200 - OK", JsonSerializer.Serialize(response));

            return Ok(response);
        }

        /// <summary>
        /// Envia código de Autenticação para o email do Adotante
        /// </summary>
        /// <param name="email">Email do Parceiro cadastrado</param>
        /// <returns>Custom Email Response</returns>
        [HttpPost("/auth/adopter/verification/email")]
        [ProducesResponseType(typeof(CustomEmailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendVerificationCodeEmail(string email)
        {
            await _adoptersVerificationCodeService.SendConfirmation(email);

            return Ok(new CustomAdoptersEmailResponse(email, StatusProcess.Success));
        }

        /// <summary>
        /// Envia código de Autenticação para o email do Adotante cadastrado
        /// </summary>
        /// <param name="email">Email do Parceiro cadastrado</param>
        /// <returns>Custom Email Response</returns>
        [HttpPost("/auth/adopter/verification/reset")]
        [ProducesResponseType(typeof(CustomEmailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendVerificationCodeEmailResetPassword(string email)
        {
            _ = await _adoptersApiService.GetAdopterByEmailAsync(email) ??
                throw new BusinessException("Email Inválido, verifique e tente novamente!",
                    StatusProcess.InvalidRequest,
                    HttpStatusCode.NotFound);

            await _adoptersVerificationCodeService.SendConfirmation(email);

            return Ok(new CustomAdoptersEmailResponse(email, StatusProcess.Success));
        }

        /// <summary>
        /// Valida o código inserido pelo usuário
        /// </summary>
        /// <param name="userId">Email do Usuário</param>
        /// <param name="code">Código inserido</param>
        /// <returns>Custom Verification Code Response de uma Token Model</returns>
        [HttpPost("/auth/adopter/verification/validate")]
        [ProducesResponseType(typeof(CustomVerificationCodeResponse<TokenModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerificationCodeValidate(string email, string code)
        {
            var isValid = await _adoptersVerificationCodeService.VerificationCodeValidate(email, code);

            return isValid ?
                Ok(new CustomAdoptersEmailResponse(email, StatusProcess.Success)) :
                throw new BusinessException("Código inválido!", StatusProcess.InvalidRequest, HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Envia confirmação de interesse para Ong
        /// </summary>
        /// <param name="request">Dados para email</param>
        /// <returns>Custom Email Response</returns>
        [HttpPost("/auth/adopter/interest/email")]
        [ProducesResponseType(typeof(CustomEmailResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendVerificationCodeEmail(AdopterSendInterestRequest request)
        {
            await _service.SendInterestEmailAsync(request.AdopterInfos, request.PetInfos, request.PartnerId);

            return Ok(new CustomEmailResponse(request.AdopterInfos.Id, StatusProcess.Success));
        }

        /// <summary>
        /// Registra um usuário com uma senha aleatório no Banco de dados
        /// </summary>
        /// <param name="request">Request com dados básicos do parceiro</param>
        /// <returns>Usuário criado</returns>
        [HttpPost("/auth/adopter/register")]
        [ProducesResponseType(typeof(CustomVerificationCodeResponse<TokenModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAsync(AdopterRegistrationRequest request)
        {
            request.Password = request.Password = _cipherService.Encrypt(request.Password);

            var entity = _mapper.Map<AdopterRegistrationRequest, Adopter>(request);

            var result = await _service.RegisterAsync(entity);

            var token = _tokenService.GenerateJwtTokenAsync(result.Id, result.Name, Roles.User, result.ProfileImageUrl);

            var response = new CustomAuthResponse<Adopter>(StatusProcess.Success, token);

            _logger.LogInformation(new EventId(), "201 - Created", JsonSerializer.Serialize(response));

            return Created(string.Empty, response);
        }

        /// <summary>
        /// Atualiza a senha de um usuário no Banco de dados
        /// </summary>
        /// <param name="request">ID e nova senha</param>
        /// <returns>Dados do usuáro atualizados</returns>
        [Authorize]
        [HttpPut("/auth/adopter/password/change")]
        [ProducesResponseType(typeof(CustomVerificationCodeResponse<TokenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangePassword(AdopterUpdatePasswordRequest request)
        {
            request.NewPassword = _cipherService.Encrypt(request.NewPassword);

            var adopter = await _service.UpdatePasswordAsync(request.Id, request.NewPassword);

            return Ok(new CustomUpdateResponse<Adopter>(adopter, StatusProcess.Success));
        }
    }
}
