using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_APIServer.Servic
{
    public  class ServicToTarget
    {
        private readonly FinalProjectDbContext _dbcontext;



        public ServicToTarget(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

        }

        public List<int> MoveTargetOnePlay(string direction, int x,int y )
        {
            int x_x = x;
            int y_y = y;

            
                switch (direction)
                {
                    case "n":
                        {
                            x_x += 1;
                            break;
                        }
                    case "s":
                        {
                            x_x -= 1;
                            break;
                        }
                    case "w":
                        {
                            y_y -= 1;
                            break;
                        }
                    case "e":
                        {
                            y_y += 1;
                            break;
                        }
                    case "nw":
                        {
                            y_y -= 1;
                            x_x += 1;
                            break;
                        }
                    case "ne":
                        {
                            y_y += 1;
                            x_x += 1;
                            break;
                        }
                    case "sw":
                        {
                            y_y -= 1;
                            x_x -= 1;
                            break;
                        }
                    case "se":
                        {
                            y_y += 1;
                            x_x -= 1;
                            break;
                        }

                }

            List<int> list = new List<int>();
            list.Add(x_x);
            list.Add(y_y);
            return list;
            }

        public async Task TaskForceCheck(Target target)
        {
            var listofagents = await _dbcontext.agnets.Include(t => t.Location).ToListAsync();

            Agent thisagent = null;
            double thisminimum = 1000;

            foreach (var agent in listofagents)
            {
                if (agent.Status == "Dormant" && agent.X != 0 && agent.Y != 0)
                {
                    double distance = Math.Sqrt(Math.Pow(target.Location.X - agent.Location.X, 2) + Math.Pow(target.Location.Y - agent.Location.X, 2));

                    if (distance <= 200)
                    {
                        if (distance <= thisminimum)
                        {
                            thisminimum = distance;
                            thisagent = agent;
                        }
                    }
                }
            }
            if (thisagent != null)
            {
                    Mission newmission = new Mission()
                    {
                        Agent = thisagent,
                        Target =  target,
                        Status = Enums.StatusMission.Proposal.ToString(),
                    };
                    _dbcontext.missions.Add(newmission);
                    _dbcontext.SaveChanges();
            }
        }

        public async Task CheckMissineTarget(Target target)
        {

            var mission = _dbcontext.missions.Include(t=> t.Target).Where(p => p.Target.Id == target.Id).ToList();



        }
    }


    }

