using Microsoft.EntityFrameworkCore;

namespace CheckOutForm.Models
{
    public class CheckOutFormContext : DbContext
    {
        public CheckOutFormContext (DbContextOptions<CheckOutFormContext> options)
            : base(options)
        {
        }

        public DbSet<CheckOutForm.Models.Devices> Devices { get; set; }

        public DbSet<CheckOutForm.Models.Rentals> Rentals { get; set; }

        public DbSet<CheckOutForm.Models.CurrentStatus> CurrentStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rentals>().ToTable("Rentals");
            modelBuilder.Entity<CurrentStatus>().ToTable("CurrentStatus");
            modelBuilder.Entity<Devices>().ToTable("Devices");
        }
    }
}
