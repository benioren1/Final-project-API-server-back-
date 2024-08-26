using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

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
                default:
                    {
                        y_y += 0;
                        x_x -= 0;
                        break;
                    }

            }

            List<int> list = new List<int>();
            list.Add(x_x);
            list.Add(y_y);
            return list;
        }



        public  double Distance(int x, int x1, int y, int y1)
        {
            return Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));
           
        }

        public async Task TaskForceCheck(Agent agnet)
        {
            var listoftargets = await _dbcontext.targets.Include(t => t.Location).ToListAsync();

            Target thistarget = null;
            double thisminimum = 1000;

            foreach (var target in listoftargets)
            {
                if (target.Status == "Live" && target.Location != null && agnet.Location != null)
                {

                    double distance = Math.Sqrt(Math.Pow(agnet.Location.X - target.Location.X, 2) + Math.Pow(agnet.Location.Y - target.Location.Y, 2));

                    if (distance <= 200)
                    {


                        if (distance <= thisminimum)
                        {
                            thisminimum = distance;
                            thistarget = target;
                        }
                    }
                }
                if (thistarget != null)
                {

                    Mission? mission = _dbcontext.missions.FirstOrDefault(p => p.Target.Id == thistarget.Id);
                    if (mission == null)

                    {

                        Mission newmission = new Mission()
                        {
                            Agent = agnet,
                            Target = thistarget,
                            Status = Enums.StatusMission.Proposal.ToString(),
                        };
                       await _dbcontext.missions.AddAsync(newmission);


                    }
                    if (mission != null && mission.Status == Enums.StatusMission.Proposal.ToString() && mission.Target.Id != thistarget.Id)
                    {
                        Mission newmission = new Mission()
                        {
                            Agent = agnet,
                            Target = thistarget,
                            Status = Enums.StatusMission.Proposal.ToString(),
                        };
                       await _dbcontext.missions.AddAsync(newmission);

                       

                    }
                }
            }
            await _dbcontext.SaveChangesAsync();

        }

        public async Task MoveAgent(Agent agent)
        {

            var missions = await _dbcontext.missions.Include(a => a.Agent).ThenInclude(l=>l.Location).Include(t => t.Target).ThenInclude(l => l.Location).Where(p => p.Status == Enums.StatusMission.Proposal.ToString()).ToListAsync();
            List<Mission> newmission = new List<Mission>();
            
            if (missions != null)
            {
                foreach (var mission in missions)
                {

                    if (mission.Agent.id == agent.id)
                    {

                        if (Distance(agent.Location.X, mission.Target.x, agent.Location.Y, mission.Target.y) > 200)
                        {
                            _dbcontext.missions.Remove(mission);
                           

                        }
                    }
                }
            }
            await _dbcontext.SaveChangesAsync();
            await TaskForceCheck(agent);

        }

        public async Task<bool> OutOfRAnge(Agent agent)
        {
            if (agent.X > 1000 || agent.Y > 1000  || agent.Y < 0 || agent.X < 0)
            {
                return true;

            }
            
            return false;
        }

    }
}

