using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_APIServer.Servic
{
    public class ServicToAgent
    {
        private readonly FinalProjectDbContext _dbcontext;



        public ServicToAgent(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

        }

        public List<int> MoveTargetOnePlay(string direction, int x, int y)
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


        public async Task TaskForceCheck(Agent agnet)
        { 
            var listoftargets = await _dbcontext.targets.Include(t=> t.Location).ToListAsync();

            Target thistarget = null;
            double thisminimum = 1000;

            foreach (var target in listoftargets)
            {
                if (target.Status == "Live" && target.x != 0 && target.y != 0)
                {
                    double distance = Math.Sqrt(Math.Pow(agnet.Location.X - target.Location.X, 2) + Math.Pow(agnet.Location.Y - target.Location.X, 2));

                if (distance <= 200)
                {

                    
                        if(distance <= thisminimum)
                        {
                            thisminimum = distance;
                            thistarget = target;
                        } 
                    }
                }
            }

            if (thistarget != null)
            {

                Mission? mission = _dbcontext.missions.FirstOrDefault(p => p.Agent.id == thistarget.Id);
                if (mission != null)
                {

                    mission.Target = thistarget;
                    _dbcontext.missions.Update(mission);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    Mission newmission = new Mission()
                    {
                        Agent = agnet,
                        Target = thistarget,
                        Status = Enums.StatusMission.Proposal.ToString(),
                    };
                    _dbcontext.missions.Add(newmission);
                    _dbcontext.SaveChanges();

                }
            }
        }


    } 

 }

