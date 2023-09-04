using OsDsII.Models;
namespace OsDsII.DTOS
{
    public record ServiceOrderDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public CustomerDTO? Customer { get; set; }
        public double Price { get; set; }
        public StatusServiceOrder Status { get; set; }
        public DateTimeOffset OpeningDate { get; set; }
        public DateTimeOffset? FinishDate { get; set;}
    }
}