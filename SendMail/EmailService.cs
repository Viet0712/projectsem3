using Project_sem3.Model;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

using MailKit.Net.Smtp;





namespace Project_sem3.SendMail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSetting;
        private IWebHostEnvironment _env;
      
        public EmailService(IOptions<EmailSetting> options,IWebHostEnvironment webHostEnvironment)
        {
            this._emailSetting = options.Value;
            _env = webHostEnvironment;
           
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builderBody = new BodyBuilder();
            DateTime timeCreate = DateTime.Now;
            string formattedDateTime = timeCreate.ToString("dd-MM-yyyy-HH:mm:ss");
            //var sourchImages = Path.Combine(_env.WebRootPath, "sourchImages");
            //var filepath = Path.Combine(sourchImages, "null.jpg");


            //byte[] fileBytes;
            //if (System.IO.File.Exists(filepath))
            //{
            //    FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            //    using (var ms = new MemoryStream())
            //    {
            //        file.CopyTo(ms);
            //        fileBytes = ms.ToArray();
            //    }
            //    builderBody.Attachments.Add("image1.jpg", fileBytes, ContentType.Parse("application/octet-stream"));
            //    //builderBody.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            //}



            //var image = builderBody.LinkedResources.Add(filepath);
            //image.ContentId = "image1";
            builderBody.HtmlBody = BuildHtml(mailRequest.UserName , mailRequest.Body,mailRequest.ToEmail, formattedDateTime);
            email.Body = builderBody.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        private string BuildHtml(string username,string body,string email , string timeCreate)
        {
         

            string htmlContent = "  <div style=\"\"background-color: #f4f4f4; padding: 20px;\"\">";
            htmlContent += $" <h2>Hello,{username}</h2>";
            htmlContent += $" <h3>{body}</h3>";
            htmlContent += $"<a href =\"http://localhost:5102/api/AuthFE/Verify/{email}/{timeCreate}/\">Click Here</a>";
            //htmlContent += "<img style=\"width:250px;height:250px\" src= \"cid:image1\"/>";
           
          
            htmlContent += " </div>";
            return htmlContent;
        }

        private string BuildHtmlForgerPassword(string username, string body, string email,string password)
        {


            string htmlContent = "  <div style=\"\"background-color: #f4f4f4; padding: 20px;\"\">";
            htmlContent += $" <h2>Hello,{username}</h2>";
            htmlContent += $" <h3>{body}</h3>";
            htmlContent += $" <p>New Passwpord: {password}</p>";
            //htmlContent += "<img style=\"width:250px;height:250px\" src= \"cid:image1\"/>";


            htmlContent += " </div>";
            return htmlContent;
        }

        private string BuildHtml2(string body)
        {

            string htmlContent = "  <div style=\"\"background-color: #f4f4f4; padding: 20px;\"\">";
            htmlContent += $" <h3>{body}</h3>";
            htmlContent += "<img style=\"width:250px;height:250px\" src= \"cid:image1\"/>";
            htmlContent += " </div>";
            return htmlContent;
        }

        public async Task SendMailForm2 (MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builderBody = new BodyBuilder();
            var sourchImages = Path.Combine(_env.WebRootPath, "sourchImages");
            var filepath = Path.Combine(sourchImages, "null.jpg");
            var FolderAttractMail = Path.Combine(_env.WebRootPath, "FileAttractMail");

            var image = builderBody.LinkedResources.Add(filepath);
            image.ContentId = "image1";
            if (mailRequest.AttractFile.Count != 0)
            {
                foreach (var item in mailRequest.AttractFile)
                {
                    byte[] bytes;

                    using (var ms = new MemoryStream())
                    {
                        item.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    builderBody.Attachments.Add(item.FileName, bytes, ContentType.Parse(item.ContentType));
                }
            }

          
            builderBody.HtmlBody = BuildHtml2(mailRequest.Body);
            email.Body = builderBody.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task ForgetPassword(MailRequest mailRequest , string password)
        {

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builderBody = new BodyBuilder();
          
         
            builderBody.HtmlBody = BuildHtmlForgerPassword(mailRequest.UserName, mailRequest.Body, mailRequest.ToEmail, password);
            email.Body = builderBody.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task ConfirmOrder(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builderBody = new BodyBuilder();


            builderBody.HtmlBody = mailRequest.Body;
            email.Body = builderBody.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


        public async Task SendMailResetPass(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            var builderBody = new BodyBuilder();
            email.Subject = mailRequest.Subject;
            builderBody.HtmlBody = mailRequest.Body;
            email.Body = builderBody.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
