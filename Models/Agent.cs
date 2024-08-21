using static System.Net.Mime.MediaTypeNames;

namespace FinalProject_APIServer.Models
{
    public class Agent
    {
        public Guid? id { get; set; }
        public string Nickname { get; set; }

        public byte[] Image { get; set; }

        public Dictionary<string, int> location { get; set;}

        public bool Status { get; set; }


        public IFormFile? setimage
        {
            get { return null; }
            set 
            {
                if (value == null) return;
                AddImage(value);
            }
        }
        public void AddImage(byte[] img)
        {
            Image = img;
        }
        public void AddImage(IFormFile img)
        {
            MemoryStream stream = new MemoryStream();
            img.CopyTo(stream);
            AddImage(stream.ToArray());
        }
    }

}
