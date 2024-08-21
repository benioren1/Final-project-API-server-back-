using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Enums;
using FinalProject_APIServer.Models;
using FinalProject_APIServer.Servic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;

namespace FinalProject_APIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController : ControllerBase
    {
        private readonly FinalProjectDbContext _dbcontext;

        private readonly ServicToTarget _servfortarget;
        //private readonly location _servfortarget;


        

        public TargetController(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;
            _servfortarget = new ServicToTarget(freindcontext);

        }

        //הצגת כל המטרות
        [HttpGet]
        public async Task<IEnumerable<Target>> GetAllTargets()
        {

            return _dbcontext.targets.ToList();

        }



        //יצירת מטרה חדש
        [HttpPost]
        public async Task<IActionResult> AddAgent(Target target)
        {

            var result = await _dbcontext.targets.AddAsync(target);
            target.Status = Enums.StatusTarget.Live.ToString();
            await _dbcontext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, new { Id =  result.Entity.Id });
        }

        //עדכון מיקום של מטרה
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> UpdateLocation(int id, location loc)
        {
            Target? thistarget =  _dbcontext.targets.FirstOrDefault(att => att.Id == id);
            thistarget.Location = loc;
            thistarget.x = loc.X;
            thistarget.y = loc.Y;
            _dbcontext.locations.Add(loc);
            await _dbcontext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, thistarget);



        }



        [HttpPut("{id}/move")]
        public async Task<IActionResult> MovingTarget(int id, MoveTarget moveone)
        {
            Target? thisagent = _dbcontext.targets.FirstOrDefault(att => att.Id == id);

            string Direction = moveone.direction;
           
               List<int?> ints =  _servfortarget.MoveTargetOnePlay(Direction, thisagent.x, thisagent.y);

                thisagent.x = ints[0];
                thisagent.y = ints[1];
           
                await _dbcontext.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, thisagent);
        }



    }
}
