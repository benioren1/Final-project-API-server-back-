namespace FinalProject_APIServer.Models
{
    public class ViewAgent
    {
        public int? id { get; set; }
        public string Nickname { get; set; }

      

        public int X { get; set; } = 0;


        public int Y { get; set; } = 0;


        public string Status { get; set; }

        public double Time_left { get; set; }


        public double   Amount_Of_Eliminations { get; set; }


    }
}
