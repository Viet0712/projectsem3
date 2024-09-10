using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Image {  get; set; }

        public bool Status { get; set; }    

        [NotMapped]
        public IFormFile? UploadImage { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set;}

        public virtual ICollection<Product>? Products { get; set; }

        public Brand()
        {
            Products = new HashSet<Product>();
        }
    }
}
