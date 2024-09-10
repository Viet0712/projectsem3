using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Order
    {
        
        public string IdOrder { get; set; }

        public int UserID { get; set; }

        public float Fee_Shipping { get; set; }

        public int StoreId { get; set; }

        public string? TypeVoucher { get; set; }

        public float? VolumeVoucher { get; set; }

        public string Status { get; set; }

        public string Address { get; set; }
        public string Phone { get; set; }

        public string Name { get; set; }
        public float Distance { get; set; }

        public string? Status_Rating { get; set; }
        public float Total {  get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public virtual User? User { get; set; }


        public virtual Store? Store { get; set; }

        public virtual Payment? Payment { get; set; } // one to one with payment

        public virtual ICollection<Order_Detail>? OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new HashSet<Order_Detail>();
        }

    }
}
