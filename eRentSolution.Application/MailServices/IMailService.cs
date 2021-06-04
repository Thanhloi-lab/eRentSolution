
using System.Threading.Tasks;
using eRentSolution.ViewModels.Utilities.Emails;

namespace eRentSolution.Application.MailServices
{
    public interface IMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
