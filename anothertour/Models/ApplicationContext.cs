using Microsoft.EntityFrameworkCore;

namespace anothertour.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Tour> Tours { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            //При первом включении
            //Database.EnsureCreated();
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Tour>().HasKey(t => t.Id);
        //}
    }
}
