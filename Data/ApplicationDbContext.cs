using CandidateHub.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateHub.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : DbContext(dbContext)
    {
        public DbSet<Candidate> Candidates { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email)
                .IsUnique(); 
        }
    }
}
