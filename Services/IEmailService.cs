namespace Portal.Services
{
    public interface IEmailService
    {
        Task<Exception> SendNotification(string email, string subject, string body);
        Task SendTestNotification(string email, string subject, string body);
    }
}
