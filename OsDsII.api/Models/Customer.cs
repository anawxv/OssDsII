using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OsDsII.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [PrimaryKey(nameof(Id))]
    [Table("customer")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id;
        [Required]
        [StringLength(60)]
        [Column("name")]
        [NotNull]
        public string Name { get; set; }

        [Required]
        [Column("email")]
        [StringLength(100)]
        [NotNull]
        public string Email { get; set; }

        [Required]
        [Column("phone")]
        [StringLength(20)]
        [AllowNull]
        public string Phone { get; set; }

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