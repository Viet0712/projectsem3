using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CodeCategory { get; set; }
        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public bool Status { get; set; }



        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<Subcategory>? Subcategories { get; set; }


        public Category()
        {
            Products = new HashSet<Product>();
            Subcategories = new HashSet<Subcategory>();

        }
    }
}
