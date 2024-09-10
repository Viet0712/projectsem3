using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Subcategory
    {
        [Key]
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CodeSubcategory { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        public virtual ICollection<Segment>? Segments { get; set; }

        public Subcategory()
        {
            Products = new HashSet<Product>();
            Segments = new HashSet<Segment>();
        }




    }
}
