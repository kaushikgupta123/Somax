using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Work_Order
{
    public class WoCancelAndPrintListModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
    }
    public class WoCancelAndPrintModel
    {
        public List<WoCancelAndPrintListModel> list { get; set; }

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

    public class WoPrintingModel
    {
        public List<WoCancelAndPrintListModel> list { get; set; }

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