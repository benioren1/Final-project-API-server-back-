using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Enums;
using FinalProject_APIServer.Models;
using FinalProject_APIServer.Servic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace FinalProject_APIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private readonly FinalProjectDbContext _dbcontext;

        private readonly ServicToTarget _servfortarget;
        //private readonly location _servfortarget;


        

        public TargetsController(FinalProjectDbContext freindcontext)
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

            if (thistarget != null)
            {
                if (thistarget.Location == null)
                {

                    thistarget.Location = loc;
                    thistarget.x = thistarget.Location.X;
                    thistarget.y = thistarget.Location.Y;
                    _dbcontext.locations.Add(loc);
                    await _dbcontext.SaveChangesAsync();
                    await _servfortarget.TaskForceCheck(thistarget);
                   
                }
            }

            return StatusCode(StatusCodes.Status200OK, thistarget);
        }


        //הוזזת מטרה בצעד אחד לפי דרישה משרת
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MovingTarget(int id, MoveTarget moveone)
        {
            Target? thistargrt = _dbcontext.targets.Include(t=> t.Location).FirstOrDefault(att => att.Id == id);

            string Direction = moveone.direction;

            if (thistargrt != null)
            {
                if (thistargrt.Location != null)
                {
                    List<int> ints = _servfortarget.MoveTargetOnePlay(Direction, thistargrt.Location.X, thistargrt.Location.Y);

                    thistargrt.x = ints[0];
                    thistargrt.y = ints[1];
                    thistargrt.Location.X = ints[0];
                    thistargrt.Location.Y = ints[1];
                }
            }

                await _dbcontext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, thistargrt);
        }



    }
}
