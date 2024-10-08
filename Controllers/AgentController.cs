﻿using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;
using FinalProject_APIServer.Servic;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace FinalProject_APIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        // מזריק לקונטרולר את החיבור לדאטה בייס
        private readonly FinalProjectDbContext _dbcontext;
        private readonly  ServicToAgent _servtoagent;

        public AgentsController(FinalProjectDbContext freindcontext)
        {
            _dbcontext = freindcontext;
            _servtoagent = new ServicToAgent(freindcontext);
        }

        //הצגת כל הסוכנים
        [HttpGet]
        public async Task<IEnumerable<ViewAgent>> GetAllAgents()
        {


              var agents =await _servtoagent.ShwoAllAgents();
                
                
              return agents;

        }



        //יצירת סוכן חדש
        [HttpPost]
        public async Task<IActionResult> AddAgent(Agent agent)
        {

            agent.Status = Enums.StatusAgent.Dormant.ToString();
            var result = await _dbcontext.agnets.AddAsync(agent);
            await _dbcontext.SaveChangesAsync();
            
            return StatusCode(StatusCodes.Status201Created,new { result.Entity.id });
        }

        //עדכון מיקום של סוכן
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> UpdateLocation(int id, location loc)
        {
            Agent? thisagent = _dbcontext.agnets.FirstOrDefault(att => att.id == id);

            if (thisagent != null)
            {
                if(thisagent.Location ==null)
                {
                    thisagent.Location = loc;
                    thisagent.X = thisagent.Location.X;
                    thisagent.Y = thisagent.Location.Y;

                    if (thisagent.X >= 0 && thisagent.Y >= 0)
                    {
                        _dbcontext.locations.Add(loc);
                        await _dbcontext.SaveChangesAsync();
                        //שליחה של הסוכן לבדיקה של האיזור שלו לביצוע חיבור משימה
                        await _servtoagent.TaskForceCheck(thisagent);
                    }
                    
                }
            }
         
            return StatusCode(StatusCodes.Status200OK, thisagent);

        }


        //הוזזת סוכן מקום אחד
        [HttpPut("{id}/move")]  
        public async Task<IActionResult> MovingTarget(int id, MoveTarget moveone)
        {
            Agent? thisagent = _dbcontext.agnets.Include(t => t.Location).FirstOrDefault(att => att.id == id);

            //תןפס את כיוון תזוזת הסוכן
            string Direction = moveone.direction;

            if (thisagent != null)
            {
                if (await _servtoagent.OutOfRAnge(thisagent))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { eror = $"this agent out of range; : P{thisagent.X} , {thisagent.Y}" });
                
                }
                //האם הסוכן בפעילות כבר
                if (thisagent.Status == Enums.StatusAgent.In_Activity.ToString())
                {

                    return StatusCode(StatusCodes.Status400BadRequest, new { eror = "this agent in activity" });
                }

                else
                {
                    //קריאה לפונקציה של שתבדוק לאיפה להוזיז את הסוכן ותחזיר לי את הערך החדש של המיקום
                    if (thisagent.Location != null && thisagent.Location != null)
                    {
                        List<int> ints = _servtoagent.MoveTargetOnePlay(Direction, thisagent.Location.X, thisagent.Location.Y);
                        if (await _servtoagent.OutOfRAnge(thisagent))
                        {
                            thisagent.X = ints[0];
                            thisagent.Y = ints[1];
                            thisagent.Location.X = ints[0];
                            thisagent.Location.Y = ints[1];
                        }
                        
                    }
                    await _servtoagent.MoveAgent(thisagent);
                }
            }
            await _dbcontext.SaveChangesAsync();
            

            return StatusCode(StatusCodes.Status200OK, thisagent);
        }


    //    var token = HttpContext.Items["Token"] as string;
    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return Unauthorized("Token is required");
    //}



}
}
