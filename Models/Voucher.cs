using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public float Volume { get; set; }

        public string Type { get; set; }

        public int Quantity { get; set; }

        public bool Status { get; set; }

        public DateTime? Start_at { get; set; }

        public DateTime? Expiry_date { get; set;}

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public virtual ICollection<VoucherUser>? VoucherUsers { get; set; }

      

        public Voucher()
        {
            VoucherUsers = new HashSet<VoucherUser>();  
           
        }


    }
}
