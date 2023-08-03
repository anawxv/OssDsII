namespace OsDsII.Models
{
    public class ServiceOrderInput
    {
        public string description { get; set; }
        public double price { get; set; }

        public BaseEntity customerId { get; set; }
    }
}