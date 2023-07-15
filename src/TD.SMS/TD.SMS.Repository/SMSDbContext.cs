using Microsoft.EntityFrameworkCore;
using TD.SMS.Contracts.Entities;

namespace TD.SMS.Repository
{
    public class SMSDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public SMSDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
