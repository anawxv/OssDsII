using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using OsDsII.DTOS.Builders;
using OsDsII.DTOS;

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
        [StringLength(200)]
        public string Description { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        [NotNull]
        public double Price { get; set; }

        [Column("status")]
        [NotMapped]
        [DefaultValue(0)]
        public StatusServiceOrder Status { get; set; }

        [Column("opening_date")]
        [Required]
        [Timestamp]
        public DateTimeOffset OpeningDate { get; set; } = DateTimeOffset.Now;

        [Column("finish_date")]
        [AllowNull]
        [Timestamp]
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
            if (CannotFinish())
            {
                throw new Exception();
            }

            Status = StatusServiceOrder.FINISHED;
            FinishDate = DateTimeOffset.Now;
        }

        public ServiceOrderDTO ToServiceOrder()
        {
            ServiceOrderDTO serviceOrderDTO = new ServiceOrderDTOBuilder()
                .WithId(Id)
                .WithDescription(Description)
                .WithPrice(Price)
                .WithStatus(Status)
                .WithOpeningDate(OpeningDate)
                .WithFinishDate()
                .WithCustomer(new CustomerDTO())
                .Build();
            return serviceOrderDTO;
        }

        public static ServiceOrder FromServiceOrderInput(ServiceOrderInput input, Customer customer)
        {
            return new ServiceOrder
            {
                Description = input.Description,
                Price = input.Price,
                Customer = customer,
                Status = StatusServiceOrder.OPEN,
                OpeningDate = DateTimeOffset.Now
            };
        }
    }
}