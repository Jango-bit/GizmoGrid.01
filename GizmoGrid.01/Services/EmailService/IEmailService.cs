namespace GizmoGrid._01.Services.EmailService
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body);
    }
}
