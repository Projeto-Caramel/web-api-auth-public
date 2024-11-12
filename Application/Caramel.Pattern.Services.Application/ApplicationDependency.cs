using Caramel.Pattern.Services.Application.Services;
using Caramel.Pattern.Services.Application.Services.Adopters;
using Caramel.Pattern.Services.Application.Services.Partners;
using Caramel.Pattern.Services.Application.Services.Security;
using Caramel.Pattern.Services.Domain.Services;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using Caramel.Pattern.Services.Domain.Services.Partners;
using Caramel.Pattern.Services.Domain.Services.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IAdoptersService, AdoptersService>();
            services.AddScoped<IAdopterVerificationCodeService, AdoptersVerificationCodeService>();

            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IPartnerVerificationCodeService, PartnersVerificationCodeService>();

            services.AddScoped<ICipherService, CipherService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}
