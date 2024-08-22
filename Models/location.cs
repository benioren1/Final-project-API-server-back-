using System.ComponentModel.DataAnnotations;

namespace FinalProject_APIServer.Models
{
    public class location
    {
        [Key]
        public int id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        


    }
}
