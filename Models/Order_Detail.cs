using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Order_Detail
    {
        [Key]
        public int Id { get; set; }
        public int PropertiesId { get; set; }

        public string OrederId { get; set; }

        public float Quantity { get; set; }

        public float Price { get; set; }

        public float? Volume_Discout { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public virtual Properties? Properties { get; set; }

        public virtual Order? Orders { get; set; }

        public virtual Rate? Rate { get; set; } 



    }
}
