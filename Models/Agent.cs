using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace FinalProject_APIServer.Models
{
    public class Agent
    {
        
        public int? id { get; set; }
        public string Nickname { get; set; }

        public string PhotoUrl { get; set; }

        
        public location? Location { get; set; }

        public int? X { get; set; } = 0;

        public int? Y { get; set; } = 0;

        public string? Status { get; set; }
    }

}
