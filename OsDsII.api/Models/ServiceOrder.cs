using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
        [Required]
        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        [NotNull]
        public double Price { get; set; }

        [Column("status")]
        [NotMapped]
        public StatusServiceOrder Status { get; set; }

        [Column("opening_date")]
        [Required]
        public DateTimeOffset OpeningDate { get; set; }

        [Column("finish_date")]
        [AllowNull]
        public DateTimeOffset FinishDate { get; set; }

        public Customer? Customer { get; set; }
        public List<Comment> Comments { get; } = new();

        public bool CanFinish()
        {
            return StatusServiceOrder.OPEN.Equals(Status);
        }

        public bool CannotFinish()
        {
            return !CanFinish();
        }

        public void FinishOS()
        {
            if(CannotFinish())
            {
                throw new Exception();
            }

            Status = StatusServiceOrder.FINISHED;
            FinishDate = DateTimeOffset.Now;
        }
    }
}