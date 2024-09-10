using System.ComponentModel.DataAnnotations;

namespace Project_sem3.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public float Volume { get; set; }


        public DateTime Start_date { get; set; }

        public DateTime End_date { get; set;}

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Properties>? Properties { get; set; }

        public Discount()
        {
            Properties = new HashSet<Properties>();
        }

    }
}
