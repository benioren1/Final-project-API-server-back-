using FinalProject_APIServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalProject_APIServer.DAL
{
    public class FinalProjectDbContext : DbContext
    {

        public DbSet<Agent> agnets { get; set; }
        

        public FinalProjectDbContext(DbContextOptions<FinalProjectDbContext> options)
            : base(options)
        {

            Console.WriteLine("Database Exists: " + Database.EnsureCreated());

            if (agnets?.Count() == 0)
            {
                Seed();
            }

        }

        public void Seed()
        { 
        
            

        
        }

        


        
    }
}
