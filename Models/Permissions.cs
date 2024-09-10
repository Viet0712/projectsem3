using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Permissions
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }

        public bool AddProperties { get; set; }

        public bool AddGoods { get; set; }

        public bool SetEven { get; set; }

        public virtual Admin? Admin { get; set; }
    }
}
