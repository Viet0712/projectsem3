using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class Product
    {
        
        public string ProductId { get; set; }


        public int? CategoryID { get; set; }

        public int? SubCategoryId { get; set; }

        public int? SegmentId {  get; set; }

        public int? BrandId { get; set; }

        public bool Status { get; set; }
        public string Name { get; set; }    

        public string Description { get; set; }

        public string? MadeIn {  get; set; }

        public string? Weight { get; set; }

        public string? Volume { get; set; }

        public string? Expiry_date { get; set; }

        public string? Shelf_life { get; set; }

        public string? Image { get; set; }

        public virtual Category? Category { get; set; }

        public virtual Subcategory? Subcategory { get; set; }

        public virtual Segment? Segment { get; set; }

        public virtual Brand? Brand { get; set; }

        public virtual ICollection<Properties>? Properties { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }
        public virtual ICollection<Product_Properties> Product_Properties { get; set; }
        public virtual ICollection<Rate>? Rates { get; set; }
        public Product()
        {
            Properties = new HashSet<Properties>();
            Questions = new HashSet<Question>();
            Rates = new HashSet<Rate>();
            Product_Properties = new HashSet<Product_Properties>();
        }

        [NotMapped]
        public List<IFormFile>? UploadImagesProduct { get; set; }

        
    }
}
