using Microsoft.EntityFrameworkCore;
using DoAn.Models;

namespace DoAn.Models
{
    // Lớp này không được để là abstract hoặc generic
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Khai báo các bảng sẽ xuất hiện trong database DoAn
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}