using System.Collections.Generic;

namespace Client.Models.Sanitation
{

    public class SanitationJobCompleteListModel
    {
        public string ClientLookupId { get; set; }
        public long SanitationJobId { get; set; }
        public string Status { get; set; }
      

    }
    public class SanitationJobCompleteModel
    {
        public List<SanitationJobCompleteListModel> list { get; set; }
        private string _comments;
        public string comments
        {
            get
            {
                return string.IsNullOrEmpty(_comments) ? "" : _comments;
            }
            set
            {
                _comments = value;
            }

        }

    }
}