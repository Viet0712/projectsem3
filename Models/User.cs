using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }    

        public string Email { get; set; }  //Unique

        public string Password { get; set; }

        public string? Rank { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? RewardPoints { get; set; }

        public string? Credit_card_number { get; set; }

        public string? Credit_card_expiry { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get;set; }

        public string? Image {  get; set; }

        public string Status { get; set; }

        public string Role { get; set; }    

        [NotMapped]

        public IFormFile? UploadImage {  get; set; }

    

        public virtual ICollection<Cart>? Carts { get; set; }

    

        public virtual ICollection<Rate>? Rates { get; set; }

        public virtual ICollection<Question>? Questions { get; set; }

        public virtual ICollection<VoucherUser>? VoucherUsers { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

        public User()
        {
          
            Carts = new HashSet<Cart>();
         
            Rates = new HashSet<Rate>();
            Questions = new HashSet<Question>();
            VoucherUsers = new HashSet<VoucherUser>();
            Orders = new HashSet<Order>();

        }


    }
}
