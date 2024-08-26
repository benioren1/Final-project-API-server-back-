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
            var listofagents = await _dbcontext.agnets
                .AsNoTracking()
                .Include(t => t.Location)
                .ToListAsync();

            Agent thisagent = null;
            double thisminimum = 1000;

            foreach (var agent in listofagents)
            {
                if (agent.Status == "Dormant" && agent.X != 0 && agent.Y != 0)
                {
                    double distance = Math.Sqrt(Math.Pow(target.Location.X - agent.Location.X, 2) + Math.Pow(target.Location.Y - agent.Location.Y, 2));

                    if (distance <= 200 && distance < thisminimum)
                    {
                        thisminimum = distance;
                        thisagent = agent;
                    }
                }
            }

            if (thisagent != null)
            {
                Mission? mission = await _dbcontext.missions
                    .FirstOrDefaultAsync(i => i.Target.Id == target.Id && i.Agent.id == thisagent.id);

                if (mission == null)
                {
                    Mission newmission = new Mission()
                    {
                        Agent = thisagent,
                        Target = target,
                        Status = Enums.StatusMission.Proposal.ToString(),
                    };
                    await _dbcontext.missions.AddAsync(newmission);
                    _dbcontext.Update(newmission);
                    await _dbcontext.SaveChangesAsync();
                }
            }
        }

        //פפונקציה לחישוב מרחק בין נקודות
        public double Distance(int x, int x1, int y, int y1)
        {
            return Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));

        }

        //פונקציה להוזיז מטרה
        public async Task MoveTarget(Target target)
        {

            var missions = await _dbcontext.missions.Include(a => a.Agent).ThenInclude(l => l.Location).Include(t => t.Target).ThenInclude(l => l.Location).Where(p => p.Status == Enums.StatusMission.Proposal.ToString()).ToListAsync();
            List<Mission> newmission = new List<Mission>();

            if (missions != null)
            {
                foreach (var mission in missions)
                {

                    if (mission.Target.Id == target.Id)
                    {
                        //בדיקה האם הסוכן יצא מטווח ה200
                        double result = Distance(target.Location.X, mission.Agent.Location.X, target.Location.Y, mission.Agent.Location.Y);

                        if (result > 200)
                        {
                            _dbcontext.missions.Remove(mission);
                           await _dbcontext.SaveChangesAsync();

                        }
                    }
                }
            }
            await TaskForceCheck(target);

        }

        //בדיקת חריגה מהגבולת של המטריצה
        public async Task<bool> OutOfRAnge(Target target)
        {
            if (target.x > 1000 || target.y > 1000 || target.y < 0 || target.x <0)
            {
                return true;

            }

            return false;
        }
    }


    }

