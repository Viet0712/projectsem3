namespace Project_sem3.Models
{
    public class CartCreate
    {
        public string Email { get; set; }

        public int PropertiesId { get; set; }

        public float Quantity { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }
    }
}
