﻿using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Requests.Partners;
using Caramel.Pattern.Services.Domain.Integrations.UsersControl.Entities.Response;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Caramel.Pattern.Integrations.UsersControl.ApiServices
{
    public class PartnerApiService : IPartnersApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenService _tokenService;
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _httpClient;

        public PartnerApiService(IHttpClientFactory clientFactory, ITokenService tokenService)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("AuthClient");
            _tokenService = tokenService;
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public async Task<Partner> GetSingleOrDefaultByIdAsync(string id)
        {
            SetAuthorization();

            var result = await _httpClient.GetAsync($"/users-control/partner?id={id}");
            var content = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                ValidateResponse(content, result.StatusCode);

            var response = JsonSerializer.Deserialize<CustomResponse<Partner>>(content, _options);

            BusinessException.ThrowIfNull(response, "CustomResponse");

            return response.Data;
        }

        public async Task<Partner> GetPartnerByEmailAsync(string email)
        {
            SetAuthorization();

            var result = await _httpClient.GetAsync($"/users-control/partner/email?email={email}");
            var content = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                ValidateResponse(content, result.StatusCode);

            var response = JsonSerializer.Deserialize<CustomResponse<Partner>>(content, _options);

            BusinessException.ThrowIfNull(response, "CustomResponse");

            return response.Data;
        }

        public async Task<Partner> RegisterPartnerAsync(PartnerRegistrationApiRequest request)
        {
            SetAuthorization();

            var result = await _httpClient.PostAsJsonAsync($"/users-control/partners", request, _options);
            var content = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                ValidateResponse(content, result.StatusCode);

            var response = JsonSerializer.Deserialize<CustomResponse<Partner>>(content, _options);

            BusinessException.ThrowIfNull(response, "CustomResponse");

            return response.Data;
        }

        public async Task<Partner> UpdatePartnerPassword(string id, PartnerUpdatePasswordApiRequest request)
        {
            SetAuthorization();

            var result = await _httpClient.PutAsJsonAsync($"/users-control/partners/password?id={id}", request, _options);
            var content = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                ValidateResponse(content, result.StatusCode);

            var response = JsonSerializer.Deserialize<CustomResponse<Partner>>(content, _options);

            BusinessException.ThrowIfNull(response, "CustomResponse");

            return response.Data;
        }

        private void ValidateResponse(string content, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.InternalServerError)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, _options);
                throw new Exception(problemDetails.Detail);
            }

            var businnesException = JsonSerializer.Deserialize<ExceptionResponse>(content, _options);
            BusinessException.ThrowIfNull(businnesException, "ExceptionResponse");

            throw new BusinessException(businnesException.ErrorDetails, businnesException.Status, statusCode);
        }

        private void SetAuthorization()
        {
            var token = _tokenService.GenerateIssuerJwtTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
