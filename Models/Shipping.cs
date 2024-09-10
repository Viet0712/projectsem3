using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Shipping
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public bool Status { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;  set; }

      

     

    }
}
