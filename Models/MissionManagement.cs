namespace FinalProject_APIServer.Models
{
    public class MissionManagement
    {

        public int? Id_MIssion { get; set; }
        public string AgentName { get; set; }

        public int Xagent { get; set; }
        public int Yagent { get; set; }

        public string TargetName  { get; set; }
        public string Position { get; set; }
        public int Xtarget { get; set; }
        public int Ytarget { get; set; }

        public double distanc {  get; set; }
        public double Time_left { get; set; }
        
        public string Status { get; set; }


    }
}
