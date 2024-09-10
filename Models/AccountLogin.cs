using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class AccountLogin
    {
        [Key]
        public string? Email { get; set; }
        public string? Password { get; set; }    
    }
}
