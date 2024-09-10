using Project_sem3.Model;

namespace Project_sem3.SendMail
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

        Task SendMailForm2(MailRequest mailRequest);

        Task SendMailResetPass(MailRequest mailRequest);
        Task ForgetPassword(MailRequest mailRequest, string password);

        Task ConfirmOrder(MailRequest mailRequest);
    }
}
