using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace OsDsII.Models
{
    [Index(nameof(Id))]
    [PrimaryKey(nameof(Id))]
    [Table("comment")]
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        [Column("id")]
        public long Id { get; set; }

        [Column("description", TypeName = "text")]
        [NotNull]
        public string Description { get; set; } = null!;

        public int ServiceOrderId { get; set; }

        [NotNull]
        [Column("send_date")]
        public DateTimeOffset SendDate { get; set; }

        public ServiceOrder ServiceOrder { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}