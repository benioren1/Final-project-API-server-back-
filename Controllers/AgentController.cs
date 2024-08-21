using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using FinalProject_APIServer.Servic;

namespace FinalProject_APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        // מזריק לקונטרולר את החיבור לדאטה בייס
        private readonly FinalProjectDbContext _dbcontext;
        private readonly  ServicToAgent _servtoagent;

        public AgentController(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;
            _servtoagent = new ServicToAgent(freindcontext);
        }

        //הצגת כל הסוכנים
        [HttpGet]
        public async Task<IEnumerable<Agent>> GetAllAgents()
        {

            return _dbcontext.agnets.ToList();

        }



        //יצירת סוכן חדש
        [HttpPost]
        public async Task<IActionResult> AddAgent(Agent agent)
        {

            agent.Status = Enums.StatusAgent.Dormant.ToString();
            var result = await _dbcontext.agnets.AddAsync(agent);
            await _dbcontext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, result.Entity);
        }

        //עדכון מיקום של סוכן
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> UpdateLocation(int id, location loc)
        {
            Agent? thisagent = _dbcontext.agnets.FirstOrDefault(att => att.id == id);
            thisagent.Location = loc;
            thisagent.x = loc.X;
            thisagent.y = loc.Y;
            _dbcontext.locations.Add(loc);
            await _dbcontext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, thisagent);

        }


        //הוזזת סוכן מקום אחד
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MovingTarget(int id, MoveTarget moveone)
        {
            Agent? thisagent = _dbcontext.agnets.FirstOrDefault(att => att.id == id);

            string Direction = moveone.direction;

            List<int?> ints = _servtoagent.MoveTargetOnePlay(Direction, thisagent.x, thisagent.y);

            thisagent.x = ints[0];
            thisagent.y = ints[1];

            await _dbcontext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, thisagent);
        }






    }
}
