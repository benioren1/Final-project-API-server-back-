using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using FinalProject_APIServer.Servic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_APIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeneralVieoController : ControllerBase
    {
        private readonly FinalProjectDbContext _dbcontext;
        

        public GeneralVieoController(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;
          
        }


        [HttpGet("GetGeneral")]
        public async Task<ActionResult<GeneralVieo>> GetAllGeneral()
        {
            GeneralVieo generalVieo = new GeneralVieo();

            generalVieo.AmountAgents = _dbcontext.agnets.Count();

            var agents = _dbcontext.agnets.Where(t => t.Status == Enums.StatusAgent.In_Activity.ToString()).ToList();

            generalVieo.TotalAgents_IS_Activity = agents.Count();

            generalVieo.AmountTarget = _dbcontext.targets.Count();

            var targets = _dbcontext.targets.Where(t => t.Status == Enums.StatusTarget.Live.ToString()).ToList();

            generalVieo.TotalTarget_IS_activity = targets.Count();

            generalVieo.AmountMission = _dbcontext.missions.Count();

            var missions = _dbcontext.missions.Where(t => t.Status == Enums.StatusMission.Conected_to_mission.ToString()).ToList();

            generalVieo.TotalMission_IS_Activity = missions.Count();

            generalVieo.Ratio_between_Agent_To_Target = _dbcontext.agnets.Count() / _dbcontext.targets.Count();

            generalVieo.Ratio_between_Agent_Able_to_target = 50;

            return Ok(generalVieo);
        }







    }
}
