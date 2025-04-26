using CleanArchitectureSystem.Application.Models.Email;

namespace CleanArchitectureSystem.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailMessage email);
    }
}
