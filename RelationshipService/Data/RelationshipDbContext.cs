using Microsoft.EntityFrameworkCore;
using RelationshipService.Entities;

namespace RelationshipService.Data
{
    public class RelationshipDbContext : DbContext
    {
        public DbSet<UserStoreRelation> UserStoreRelations { get; set; }
        public DbSet<PaymentOrderRelation> PaymentOrderRelations { get; set; }

        public RelationshipDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserStoreRelation>()
                .HasKey(usr => new { usr.UserId, usr.StoreId });

            modelBuilder.Entity<PaymentOrderRelation>()
                .HasKey(paymentOrder => new { paymentOrder.PaymentId, paymentOrder.OrderId });
        }
    }
}
