using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;

namespace FinalProject_APIServer.Servic
{
    public class ServicToTarget
    {
        private readonly FinalProjectDbContext _dbcontext;



        public ServicToTarget(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

        }

        public void MoveTargetOnePlay(string direction, Target? target)
        {
            if (target != null)
            {
                switch (direction)
                {
                    case "n":
                        {
                            target.x += 1;
                            break;
                        }
                    case "s":
                        {
                            target.x -= 1;
                            break;
                        }
                    case "w":
                        {
                            target.y -= 1;
                            break;
                        }
                    case "e":
                        {
                            target.y += 1;
                            break;
                        }
                    case "nw":
                        {
                            target.y -= 1;
                            target.x -= 1;
                            break;
                        }
                    case "ne":
                        {
                            target.y += 1;
                            target.x += 1;
                            break;
                        }
                    case "sw":
                        {
                            target.y -= 1;
                            target.x -= 1;
                            break;
                        }
                    case "se":
                        {
                            target.y += 1;
                            target.x -= 1;
                            break;
                        }

                }
                _dbcontext.SaveChanges();
            }
            
        
            
        
        }


    }
}
