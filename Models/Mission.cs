using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject_APIServer.Models
{
    public class Mission
    {
        [Key]
        public int? Id { get; set; }
        public Agent Agent { get; set; }
        public Target Target { get; set; }
        public int? Time_left { get; set; }
        public DateTime? At_Time { get; set; }
        public string Status { get; set; }
        public Mission() { }
    }
}
