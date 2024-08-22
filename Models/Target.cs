using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace FinalProject_APIServer.Models
{
    public class Target
    {
        [Key]
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

  
        public location? Location { get; set; }
        public int x { get; set; } = 0;

        public int y { get; set; } = 0;
        [NotMapped]
        public string PhotoUrl { get; set; }

        public string? Status { get; set; }

        public Target() { }

    }
}
