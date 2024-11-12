using Caramel.Pattern.Services.Domain.Entities.DTOs.Responses;
using Caramel.Pattern.Services.Domain.Enums;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Caramel.Pattern.Services.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocorreu uma Exceção: {Message}", exception.Message);

            var problemDetails = new ExceptionResponse(StatusProcess.Failure, [exception.Message]);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }
    }
}
