using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }

        public string ProductId { get; set; }

        public int Order_detailId { get; set; }

       

        public int UserId { get; set; }

        public int? Rating { get; set; }

        public string? Content { get; set; }

        public bool? Status { get; set; }
        public int? Like {  get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public virtual Product? Products { get; set; }

        public virtual User? User { get; set; }

        public virtual Rate_Reply? Rate_Replies { get; set; }

        public virtual Order_Detail? Order_Detail { get; set; }


    }
}
