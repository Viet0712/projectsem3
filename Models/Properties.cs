using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class Properties
    {
        [Key]
        public int Id { get; set; }

        public string ProductId { get; set; }

        public int StoreId { get; set; }

        public int? DiscountId { get; set; }
            
        public int? FlashSaleId { get; set; }

        public string? Image {  get; set; }

        public float CostPrice { get; set; }

        [NotMapped]
        public IFormFile? UpLoadImage { get; set; }

        public float Price { get; set; }

      
        
        public string Name { get; set; }

        public bool Status { get; set; }

      

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }




        public virtual Product? Product { get; set; }

        public virtual Store? Store { get; set; }

        public virtual Discount? Discount { get; set; }

        public virtual Flash_Sale? Flash_Sale { get; set; }




        public virtual ICollection<Cart>? Carts { get; set; }
       

        public virtual ICollection<Order_Detail>? OrderDetails { get; set; }

        public virtual ICollection<Goods>? Goods { get; set; }
        public Properties()
        {
            Carts = new HashSet<Cart>();
          
            OrderDetails = new HashSet<Order_Detail>();
            Goods = new HashSet<Goods>();

        }
    }
}
