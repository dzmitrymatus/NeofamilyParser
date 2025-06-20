using Microsoft.EntityFrameworkCore;
using NeofamilyParser.DAL.Models;

namespace NeofamilyParser.DAL
{
    public class NeofamilyParserDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }

        public NeofamilyParserDbContext() : base() { }

        public NeofamilyParserDbContext(DbContextOptions<NeofamilyParserDbContext> options)
           : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>();
        }
    }
}
