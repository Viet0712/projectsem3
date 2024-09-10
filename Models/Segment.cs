using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Segment
    {
        [Key]
        public int Id { get; set; }

        public int SubCategoryId { get; set; }

        public string Name { get; set; }

        public string CodeSegment { get; set; }

        public bool Status { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public virtual Subcategory? Subcategory { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        public Segment()
        {
            
            Products = new HashSet<Product>();
        }
    }
}
