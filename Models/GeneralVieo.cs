namespace FinalProject_APIServer.Models
{
    public class GeneralVieo
    {
        public int AmountAgents { get; set; }

        public int TotalAgents_IS_Activity { get; set; }
        public int AmountTarget { get; set; }
        public int TotalTarget_IS_activity { get; set; }

        public int AmountMission { get; set; }

        public int TotalMission_IS_Activity { get; set; }

        public double Ratio_between_Agent_To_Target { get; set; }
        public double Ratio_between_Agent_Able_to_target { get; set; }
    }
}
