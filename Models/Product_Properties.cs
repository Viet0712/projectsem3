using MimeKit.Encodings;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class Product_Properties
    {
        [Key]
        public int Id { get; set; }

        public string ProductId { get; set; }
        public string? Image { get; set; }

        public float CostPrice { get; set; }

     

        public float Price { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }



        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }


        [NotMapped]
        public IFormFile? UpLoadImage { get; set; }




        public virtual Product? Product { get; set; }
    }
}
