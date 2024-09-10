namespace Project_sem3.Models
{
    public class PaymentRequest
    {
      
            public List<int>? ListCart { get; set; }
            public int? Voucher { get; set; }

        public float Fee_Shipping { get; set; }
       
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }

        public int Distance { get; set; }

        public float Total { get; set; }
    }
}
