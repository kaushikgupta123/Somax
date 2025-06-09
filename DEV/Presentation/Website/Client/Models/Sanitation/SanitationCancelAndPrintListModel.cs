using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Sanitation
{
    public class SanitationCancelAndPrintListModel
    {
        public long SanitationJobId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "CancelReasonErrormsg|"+ LocalizeResourceSetConstants.Global)]
        public string CancelReason { get; set; }
        public string Comments { get; set; }
    }
    public class SanitationCancelAndPrintModel
    {
        public List<SanitationCancelAndPrintListModel> list { get; set; }

        private string _cancelreason;
        public string cancelreason
        {
            get
            {
                return string.IsNullOrEmpty(_cancelreason) ? "" : _cancelreason;
            }
            set
            {
                _cancelreason = value;
            }

        }
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
    public class SjPrintingModel
    {
        public List<SanitationCancelAndPrintListModel> list { get; set; }

        private string _cancelreason;
        public string cancelreason
        {
            get
            {
                return string.IsNullOrEmpty(_cancelreason) ? "" : _cancelreason;
            }
            set
            {
                _cancelreason = value;
            }

        }
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
        private string _PrintingCountConnectionID;
        public string PrintingCountConnectionID
        {
            get
            {
                return string.IsNullOrEmpty(_PrintingCountConnectionID) ? "" : _PrintingCountConnectionID;
            }
            set
            {
                _PrintingCountConnectionID = value;
            }

        }
    }
}