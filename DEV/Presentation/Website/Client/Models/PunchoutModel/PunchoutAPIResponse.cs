namespace Client.Models.PunchoutModel
{
    public class PunchoutAPIResponse
    {
        public int ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseURL { get; set; }
    }
}