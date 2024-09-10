using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public bool Status { get; set; }

        public string? Image { get; set; }

        [NotMapped]

        public IFormFile? UploadImage { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public virtual ICollection<Admin>? Admins { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

        public virtual ICollection<Properties>? Properties { get; set; }

        public Store()
        {
            Admins = new HashSet<Admin>();
            Orders = new HashSet<Order>();
            Properties = new HashSet<Properties>();
        }
    }
}
