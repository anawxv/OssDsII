using System.Diagnostics.CodeAnalysis;

namespace OsDsII.Models
{
    public class BaseEntity
    {
        [NotNull]
        public int Id { get; set; }
    }
}