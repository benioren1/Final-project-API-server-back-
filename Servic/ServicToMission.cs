using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_APIServer.Servic
{
    public class ServicToMission
    {


        private readonly FinalProjectDbContext _dbcontext;



        public ServicToMission(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

        }

        public async Task MoveMission()
        {
            var missions = await _dbcontext.missions
                .AsNoTracking() 
                .Include(a => a.Agent).ThenInclude(l => l.Location)
                .Include(t => t.Target).ThenInclude(t => t.Location)
                .Where(t => t.Status == Enums.StatusMission.Conected_to_mission.ToString())
                .ToListAsync();

            foreach (var mission in missions)
            {
                await MoveMission1(mission.Target, mission.Agent);
                await FinishTheMission(mission);
            }
        }


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
            
            target.x = target.Location.X;
            target.y = target.Location.Y;
            agent .X = agent.Location.X;
            agent.Y = agent.Location.Y;
            _dbcontext.Update(agent);
            _dbcontext.Update(target);
            await _dbcontext.SaveChangesAsync();
        }

        public double Distance(int x, int x1, int y, int y1)
        {
            return Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));

        }

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
                    mission.At_Time = DateTime.Now;
                }
              
            }
            _dbcontext.Update(mission);

            await _dbcontext.SaveChangesAsync();
        }

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
            await _dbcontext.SaveChangesAsync() ;
        
        }
    }
}
