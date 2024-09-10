using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }


        public int? StoreId { get; set; }

        public string? Image {  get; set; }

        public string Role {  get; set; }
        public bool Status { get; set; }

        public bool IsOnline { get; set; }
        public virtual Store? Store { get; set; }

        public virtual ICollection<Rate_Reply> Rate_Reply { get; set; }

        public virtual ICollection<Question_Reply> Question_Reply { get; set; }

        public virtual Permissions? Permissions { get; set; }

        public Admin()
        {
            Rate_Reply = new HashSet<Rate_Reply>();
            Question_Reply = new HashSet<Question_Reply>();
        }
        [NotMapped]
        public IFormFile? UploadImage { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set;}
    }
}
