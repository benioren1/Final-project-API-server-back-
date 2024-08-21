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

        

        public TargetController(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;

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
            if (thisagent != null)
            {
                //_servfortarget.MoveTargetOnePlay(Direction, thisagent);
                switch (Direction)
                {
                    case "n":
                        {
                            thisagent.x += 1;
                            break;
                        }
                    case "s":
                        {
                            thisagent.x -= 1;
                            break;
                        }
                    case "w":
                        {
                            thisagent.y -= 1;
                            break;
                        }
                    case "e":
                        {
                            thisagent.y += 1;
                            break;
                        }
                    case "nw":
                        {
                            thisagent.y -= 1;
                            thisagent.x -= 1;
                            break;
                        }
                    case "ne":
                        {
                            thisagent.y += 1;
                            thisagent.x += 1;
                            break;
                        }
                    case "sw":
                        {
                            thisagent.y -= 1;
                            thisagent.x -= 1;
                            break;
                        }
                    case "se":
                        {
                            thisagent.y += 1;
                            thisagent.x -= 1;
                            break;
                        }
                       
                }
                await _dbcontext.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, thisagent);

            }
            else return StatusCode(StatusCodes.Status200OK, thisagent);


        }



    }
}
