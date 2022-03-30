using CommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
namespace CommerceApi.DatabaseContext
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
                : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        public DbSet<Sale> sales { get; set; }
        public DbSet<UserClass> userClass { get; set; }
        
    }
}
