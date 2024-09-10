using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Flash_Sale
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public float Volume { get; set; }

        public DateTime? End_Date { get; set; }

        public DateTime? Start_Date { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Properties>? Properties { get; set; }

        public Flash_Sale()
        {
            Properties = new HashSet<Properties>();
        }




    }
}
