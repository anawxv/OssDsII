namespace OsDsII.DTOS
{
    public record CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email {get; set;}
    }
}