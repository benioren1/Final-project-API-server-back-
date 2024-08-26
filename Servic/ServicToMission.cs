using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_APIServer.Servic
{
    public class ServicToMission
    {


        private readonly FinalProjectDbContext _dbcontext;


        // פה אני מזריק db 
        public ServicToMission(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

        }

        //פונקציה שמשתמשת בעוד פונקציות לצורך הוזזה של סוכן כאן היא מביאה רשימה של משימות רלוונטיות 
        public async Task MoveMission()
        {
            var missions = await _dbcontext.missions
       .AsNoTracking() 
       .Include(m => m.Agent)
           .ThenInclude(a => a.Location)
       .Include(m => m.Target)
           .ThenInclude(t => t.Location)
       .Where(m => m.Status == Enums.StatusMission.Conected_to_mission.ToString())
       .ToListAsync();

            foreach (var mission in missions)
            {
                await MoveMission1(mission.Target, mission.Agent);
                await FinishTheMission(mission);
            }
        }

        //פונקציה שמזיזה את הסוכן לכיוון המטרה 
        public async Task MoveMission1(Target target, Agent agent)
        {
            if (target.Location != null && agent.Location != null)
            {
                if (agent.Location.X > target.Location.X)
                    agent.Location.X--;
                if (agent.Location.X < target.Location.X)
                    agent.Location.X++;
                if (agent.Location.Y > target.Location.Y)
                    agent.Location.Y--;
                if (agent.Location.Y < target.Location.Y)
                    agent.Location.Y++;
                    
                 
            }
            agent .X = agent.Location.X;
            agent.Y = agent.Location.Y;
            _dbcontext.Update(agent);
            _dbcontext.Update(target);
            await _dbcontext.SaveChangesAsync();
        }

        //פונקציה שבודקת מרחק בין נקודות במטריצה
        public double Distance(int x, int x1, int y, int y1)
        {
            return Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));

        }

        public double DistanceTime(int x, int x1, int y, int y1)
        {
            return (Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2))) / 5.0;

        }

        //בדיקה האם המטרה חוסלה
        public async Task FinishTheMission(Mission mission)
        {
            if (mission.Agent.Location != null && mission.Target.Location != null)
            {
                if (mission.Agent.Location.X == mission.Target.Location.X &&
                    mission.Agent.Location.Y == mission.Target.Location.Y)
                {

                    mission.Status = Enums.StatusMission.Ended.ToString();
                    mission.Target.Status = Enums.StatusTarget.Eliminated.ToString();
                    mission.Agent.Status = Enums.StatusAgent.Dormant.ToString();
                    _dbcontext.Update(mission);
                    _dbcontext.targets.Update(mission.Target);
                    _dbcontext.agnets.Update(mission.Agent);
                }
              
            }
            

            await _dbcontext.SaveChangesAsync();
        }

        // מחיקת כל המשימות הלא רלוונטיות יותר 
        public async Task RemoveMission(Mission mission)
        { 
            var missions =await _dbcontext.missions.Include(a => a.Agent).Include(t=>t.Target).Where(t=> t.Status == Enums.StatusMission.Proposal.ToString()).ToListAsync();

            foreach (var missionn in missions)
            {
                if (missionn.Target.Id == mission.Target.Id || missionn.Agent.id == mission.Agent.id)
                {
                    _dbcontext.missions.Remove(missionn);
                }
            }
            _dbcontext.Update(mission);
            await _dbcontext.SaveChangesAsync() ;
        
        }
    }
}
