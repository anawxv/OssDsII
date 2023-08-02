using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OsDsII.Models;

namespace OsDsII.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(customer => customer.Email)
                .IsUnique();

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(entity => entity.Customer)
                .WithMany()
                .IsRequired();

            modelBuilder.Entity<ServiceOrder>()
                .HasMany(service => service.Comments)
                .WithOne(e => e.ServiceOrder)
                .HasForeignKey(entity => entity.ServiceOrderId)
                .IsRequired();
            modelBuilder.Entity<ServiceOrder>()
            .Property(serviceOrder => serviceOrder.Status)
            .HasConversion(new EnumToStringConverter<StatusServiceOrder>());

            modelBuilder.Entity<Comment>()
                .HasOne(e => e.ServiceOrder)
                .WithMany(e => e.Comments)
                .IsRequired();

        }
    }
}