using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Question_Reply
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public int AdminId { get; set; }

        public string Content { get; set; }

        public bool? Status { get; set; }
        public int Like { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public virtual Question? Question { get; set; }

        public virtual Admin? Admin { get; set; }  
    }
}
