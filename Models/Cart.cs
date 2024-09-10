using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }

        public int PropertiesId { get; set; }

        public float Quantity { get; set; }

        public DateTime? Create_at {  get; set; }

        public DateTime? Update_at { get; set; }



        public virtual User? User { get; set; }

        public virtual Properties? Properties { get; set; }

    }
}
