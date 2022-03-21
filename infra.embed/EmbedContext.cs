using Microsoft.EntityFrameworkCore;
using model.Entities;

namespace infra.embed
{
    public class EmbedContext : DbContext
    {
        public EmbedContext(DbContextOptions<EmbedContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscriber>().HasData(
                new Subscriber("Sub1", "Sub1@email.com"),
                new Subscriber("Sub2", "Sub2@email.com"),
                new Subscriber("Sub3", "Sub3@email.com")
                );


            base.OnModelCreating(modelBuilder);
        }
    }
}