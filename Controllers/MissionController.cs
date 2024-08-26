using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using FinalProject_APIServer.Servic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_APIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {


        private readonly FinalProjectDbContext _dbcontext;
        private readonly ServicToMission _servtomis;

        public MissionsController(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;
            _servtomis = new ServicToMission(freindcontext);
        }

        [HttpGet]
        public async Task<IEnumerable<MissionManagement>> GetAllMissionsProposal()
        {
            var listmission = await _dbcontext.missions.Include(a=>a.Agent).ThenInclude(l=>l.Location).Include(t=>t.Target).ThenInclude(l=>l.Location).ToListAsync();
            List<MissionManagement> missions = new List<MissionManagement>();
            foreach (var mission in listmission)
            {

                MissionManagement mis= new MissionManagement();
                mis.Id_MIssion = mission.Id;
                mis.AgentName = mission.Agent.Nickname;
                mis.Xagent = mission.Agent.Location.X;
                mis.Yagent = mission.Agent.Location.Y;
                mis.TargetName = mission.Target.Name;
                mis.Position = mission.Target.Position;
                mis.Xtarget = mission.Target.Location.X;
                mis.Ytarget = mission.Target.Location.Y;
                mis.distanc = _servtomis.Distance(mission.Target.Location.X, mission.Agent.Location.X, mission.Target.Location.Y, mission.Agent.Location.Y);
                mis.Time_left = 50;
                mis.Status = mission.Status;

                missions.Add(mis);

            }

            return missions;
        }


        // משנה סטטוס של משימה לפעילה ומצוותת
        [HttpPut("{id}")]
        
        public async Task ChangeStatus(int id)
        { 
        
            Mission? mission = _dbcontext.missions.Include(a => a.Agent).ThenInclude(l => l.Location).Include(t => t.Target).ThenInclude(t => t.Location).FirstOrDefault(s => s.Id == id);

            if (mission != null)
            {
               double result =  _servtomis.Distance(mission.Agent.Location.X, mission.Target.Location.X, mission.Agent.Location.Y, mission.Target.Location.Y);
                if (result > 200)
                {
                    // מוחק במידה והטווח גדל ביניהם מעל 200
                    _dbcontext.missions.Remove(mission);
                }
                else
                {

                    //כאן משנה את הסטטוס של שניהם
                    mission.Status = Enums.StatusMission.Conected_to_mission.ToString();
                    mission.Agent.Status = Enums.StatusAgent.In_Activity.ToString();
                    _dbcontext.SaveChanges();
                    await _servtomis.RemoveMission(mission);
                }
                _dbcontext.SaveChanges();
            }
        }


        //עדכון מיקום של כל הסוכנים במשימות לפי הנתיב שלהם
     
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateMissions()
        {
            await _servtomis.MoveMission();
            return Ok(); 
        }
        


    }
}
