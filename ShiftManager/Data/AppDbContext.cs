using Microsoft.EntityFrameworkCore;
using ShiftManager.Models;

namespace ShiftManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job_Shift>().HasKey(js => new
            {
                js.JobId,
                js.ShiftId
            });

            modelBuilder.Entity<Job_Shift>().HasOne(s => s.Shift).WithMany(js => js.Jobs_Shifts).HasForeignKey(s => s.ShiftId);
            modelBuilder.Entity<Job_Shift>().HasOne(s => s.Job).WithMany(js => js.Jobs_Shifts).HasForeignKey(s => s.JobId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Job_Shift> Shifts_Jobs { get; set; }
    }
}
