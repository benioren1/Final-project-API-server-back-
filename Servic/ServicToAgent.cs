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
            var listoftargets = _dbcontext.targets.ToList();

            Target thistarget = null;
            double thisminimum = 0;

            foreach (var target in listoftargets)
            {
                
                double distance = Math.Sqrt(Math.Pow(agnet.x - target.x, 2) + Math.Pow(agnet.y - target.y, 2));

                if (distance <= 200)
                {

                    if (target.Status == Enums.StatusTarget.Live.ToString())
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

                Mission? mission = _dbcontext.missions.FirstOrDefault(p => p.Target.Id == thistarget.Id);
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

