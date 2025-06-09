namespace Client.Models.FleetService
{
    public class LineItemDeleteParameter
    {
        public bool DeleteAllowed { get; set; }
        public bool LabourExists { get; set; }
        public bool PartExists { get; set; }
        public bool OthersExists { get; set; }

    }
}