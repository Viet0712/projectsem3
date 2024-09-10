using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string OrderId { get; set; }

        public float Revenue { get; set; }

        public DateTime? Create_at { get; set; }
        public DateTime? Update_at { get; set;}

        public virtual Order? Order { get; set; }
    }
}
