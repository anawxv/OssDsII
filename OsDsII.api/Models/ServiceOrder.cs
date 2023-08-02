using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace OsDsII.Models
{
    [PrimaryKey(nameof(Id))]
    [Table("service_order")]
    public class ServiceOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [NotNull]
        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        public double Price { get; set; }

        [Column("status")]
        [NotMapped]
        public StatusServiceOrder Status { get; set; }

        [Column("opening_date")]
        public DateTimeOffset openingDate { get; set; }

        [Column("finish_date")]
        public DateTimeOffset finishDate { get; set; }

        // many to one
        public Customer Customer { get; set; } = null!;
        public List<Comment> Comments = new();
    }
}