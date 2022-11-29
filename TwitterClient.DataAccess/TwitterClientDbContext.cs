using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TwitterClient.Domain; 

namespace TwitterClient.DataAccess
{
    public class TwitterClientDbContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }

       
        public TwitterClientDbContext(DbContextOptions<TwitterClientDbContext> options) : base(options)
        {
        }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tweet>().ToTable("Tweet");
        }
    }

}