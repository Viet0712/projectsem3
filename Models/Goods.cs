using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Goods
    {
        [Key]
        public int Id { get; set; }

        public int PropertiesId { get; set; }

        public DateTime Arrival_date { get; set; }

        public DateTime? Expiry_date { get; set; }

        public int Stock { get; set; }

        public bool? Status { get; set; }

        public virtual Properties? Properties { get; set; }
    }
}
