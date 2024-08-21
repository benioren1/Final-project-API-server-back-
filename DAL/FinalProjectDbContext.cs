using FinalProject_APIServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalProject_APIServer.DAL
{
    public class FinalProjectDbContext : DbContext
    {

        public DbSet<Agent> agnets { get; set; }
        public DbSet<Target> targets { get; set; }
        public DbSet<Mission> missions { get; set; }
        public DbSet<location> locations { get; set; }
        
        

        public FinalProjectDbContext(DbContextOptions<FinalProjectDbContext> options)
            : base(options)
        {

            Console.WriteLine("Database Exists: " + Database.EnsureCreated());

            if (agnets?.Count() == 0)
            {
                Seed();
            }
            if (targets?.Count() == 0)
            {
                Seed1();
            }


        }

        public void Seed()
        {
            


        //Agent Agent1 = new Agent()
        //    {

        //        Nickname = "ben",
        //       x=45,
        //       y=12,
                
        //        Status = true,
        //    };
        //    agnets.Add(Agent1);
        //    SaveChanges();
        return;

        }
        public void Seed1()
        {


            //Target target1 = new Target()
            //{

            //    Name = "muhamad",
            //    Role = "officer",
            //    x=78,
            //    y=12,
            //    Status = true,
            //};
            //targets.Add(target1);
            //SaveChanges();
            return;
        }





    }
}
