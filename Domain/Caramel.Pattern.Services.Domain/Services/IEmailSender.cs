namespace Caramel.Pattern.Services.Domain.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receiver, string body, string subject, string cc = null);
    }
}
