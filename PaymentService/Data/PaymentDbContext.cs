using Microsoft.EntityFrameworkCore;
using PaymentService.Entities;

namespace PaymentService.Data
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>()
                .HasMany(p => p.PaymentTransactions)
                .WithOne(pt => pt.Payment)
                .HasForeignKey(pt => pt.PaymentId);
        }
    }
}
