using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Model
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string? UserName { get; set; }

        [NotMapped]

        public List<IFormFile>? AttractFile { get; set; }

   
    }
}
