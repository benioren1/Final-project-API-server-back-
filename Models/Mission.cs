using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject_APIServer.Models
{
    public class Mission
    {
        [Key]
        public int Id { get; set; }
        public int Agent_id { get; set; }
        public int Target_id { get; set; }
        public int time_left { get; set; }
        public DateTime At_Time { get; set; }
        public Mission() { }
    }
}
