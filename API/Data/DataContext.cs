using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entites;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        protected string _connectionString = "Server=localhost\\SQLEXPRESS;Database=TravelAppDB;Trusted_Connection=True;";
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Trip> Trips {get; set; }
        public DbSet<Passenger> Passenger {get; set;}
        public DbSet<Car> Cars {get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                 modelBuilder.Entity<AppUser>()
                .Property(r => r.Email)
                .IsRequired();
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}