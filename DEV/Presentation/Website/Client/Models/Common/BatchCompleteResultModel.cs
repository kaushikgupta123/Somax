using System.Collections.Generic;

namespace Client.Models.Common
{
    public class BatchCompleteResultModel
    {
        public BatchCompleteResultModel()
        {
            ErrMsg = new List<string>();
        }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public List<string> ErrMsg { get; set; }
    }
}