using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using FinalProject_APIServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Azure.Core;
using NuGet.Common;

namespace FinalProject_APIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private static readonly Dictionary<string, string> UserTokens = new Dictionary<string, string>();

        private string GenerateToken(string userip)
        {
            var tokenhendler = new JwtSecurityTokenHandler();
            string secretkey = "1234dyi5fjthgjdndfadsfgdsjfgj464twiyyd5ntyhgkdrue74hsf5ytsusefh55678";
            byte[] key = Encoding.ASCII.GetBytes(secretkey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                    new Claim(ClaimTypes.Name,userip)

                    }

                    ),
                Expires = DateTime.Now.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature

                )
            };
            
            var token = tokenhendler.CreateToken(tokenDescriptor);
            
            var tokenString = tokenhendler.WriteToken(token);

            return tokenString;


        }


        [HttpPost]
        public IActionResult Login(Loggin loggin)
        {
            if (loggin.id == "Mvc")
            {

                string userIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();


                UserTokens[loggin.id] = GenerateToken(userIP);

                return StatusCode(200
                    , new { token = GenerateToken(userIP) }
                    );
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                    new { error = "invalid credentials" });
        }


        //[HttpPost]
        //public async  Task< IActionResult> Login( Loggin request)
        //{
        //    if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        //    {
        //        return BadRequest("Invalid request");
        //    }

        //    if (request.Username == "user" && request.Password == "password")
        //    {

        //        var token = Guid.NewGuid().ToString();
        //        UserTokens[request.Username] = token; 


        //        return Ok(new { Token = token });
        //    }

        //    return Unauthorized();
        //}


    }
}
