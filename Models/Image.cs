using System.ComponentModel.DataAnnotations;

namespace FinalProject_APIServer.Models
{
    public class Image
    {
        [Key]

        public int Id { get; set; }

        public Agent Freind { get; set; }
        
        public byte[] MyImage { get; set; }
    }
}
