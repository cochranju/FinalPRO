using Microsoft.EntityFrameworkCore;

namespace FinalPRO.Models
{
    public class ApplicationDbContext : DbContext
    {
        // Set the database connection string
        private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyApplicationDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        // Override the DbContext.OnConfiguring method to set the database provider and connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        // Define the DbSet properties for each entity in the database
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Agency> Agencies { get; set; }
    }
}
