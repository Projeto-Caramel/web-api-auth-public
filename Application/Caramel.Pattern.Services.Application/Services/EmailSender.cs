using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Caramel.Pattern.Services.Domain.Enums;
using Caramel.Pattern.Services.Domain.Exceptions;
using Caramel.Pattern.Services.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Caramel.Pattern.Services.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailSender : IEmailSender
    {
        private readonly AmazonSimpleEmailServiceClient _client;

        public EmailSender(IConfiguration configuration)
        {
            var accessKey = configuration["EmailSettings:AccessKey"];
            var secretKey = configuration["EmailSettings:SecretKey"];

            _client = new AmazonSimpleEmailServiceClient(
                accessKey,
                secretKey,
                region: RegionEndpoint.SAEast1);
        }

        public async Task SendEmailAsync(string receiver, string body, string subject, string cc = null)
        {
            try
            {
                var response = await _client.SendEmailAsync(
                    new SendEmailRequest
                    {
                        Destination = new Destination
                        {
                            ToAddresses = new() { receiver },
                            CcAddresses = cc is null ? null : new() { receiver },
                        },
                        Message = new Message
                        {
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = body
                                }
                            },
                            Subject = new Content
                            {
                                Charset = "UTF-8",
                                Data = subject
                            }
                        },

                        Source = "equipecaramel@projetocaramel.com.br"
                    });
            }
            catch (Exception error)
            {
                throw new BusinessException(error.Message, StatusProcess.SESFailure, HttpStatusCode.InternalServerError);
            }
        }

    }
}
