using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class VoucherUser
    {
        [Key]
        public int Id { get; set; } 
        public int VoucherId { get; set; }

        public int UserId { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public bool? Status { get; set; }

        public virtual Voucher? Voucher { get; set; }

        public virtual User? User { get; set; }
    }
}
